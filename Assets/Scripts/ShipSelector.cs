using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Selection box code: https://gamedevacademy.org/rts-unity-tutorial/
public class ShipSelector : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public ShipListScriptableObject selectedShips;
    public PlayerInputScriptableObject playerInput;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private bool isStore;
    private Vector2 _startPos;
    private Vector2 _mousePos;
    private Vector2 _projectedMousePos;
    private bool _drawSelectionBox = false;
    private bool _dragShips = false;
    private CinemachineBrain _brain;
    private CinemachineVirtualCamera _virtualCamera;
    private static ContactFilter2D PlayerFilter;
    
    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
        PlayerFilter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("PlayerShips"),
            useLayerMask = true
        };
    }
    private void OnEnable()
    {
        _playerInputActions.Enable();
        _brain = FindObjectOfType<CinemachineBrain>();
        _virtualCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    public void EnableCombatInput()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Combat.Enable();
        Time.timeScale = 1f;
    }
    public void EnableStoreInput()
    {
        _playerInputActions.Combat.Disable();
        _playerInputActions.UI.Enable();
        Time.timeScale = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.Combat.Pause.started += Pause;
        _playerInputActions.Combat.LeftClick.started += CombatLeftClick;
        _playerInputActions.Combat.LeftClick.performed += CombatLeftClick;
        _playerInputActions.Combat.LeftClick.canceled += CombatLeftClick;

        _playerInputActions.Combat.RightClick.started += CombatRightClick;
        _playerInputActions.Combat.RightClick.performed += CombatRightClick;
        _playerInputActions.Combat.RightClick.canceled += CombatRightClick;
        
        _playerInputActions.UI.LeftClick.started += StoreLeftClick;
        _playerInputActions.UI.LeftClick.performed += StoreLeftClick;
        _playerInputActions.UI.LeftClick.canceled += StoreLeftClick;
        
        _playerInputActions.UI.RightClick.started += StoreRightClick;
        _playerInputActions.UI.RightClick.performed += StoreRightClick;
        _playerInputActions.UI.RightClick.canceled += StoreRightClick;

        if (isStore)
        {
            EnableStoreInput();
        }
        else
        {
            EnableCombatInput();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = _playerInputActions.Mouse.Point.ReadValue<Vector2>();
        _projectedMousePos = Camera.main.ScreenToWorldPoint(_mousePos);
        if (_drawSelectionBox)
        {
            UpdateSelectionBox();
        }
        if (_dragShips)
        {
            foreach (GameObject ship in selectedShips.ShipList)
            {
                if (ship != null)
                {
                    ShipLogic battleController = ship.GetComponent<ShipLogic>();
                    ship.transform.position = _projectedMousePos + battleController.positionOffset;
                }
            }
        }
    }

    private void UpdateSelectionBox()
    {
        if(!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);
        float width = _mousePos.x - _startPos.x;
        float height = _mousePos.y - _startPos.y;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.position = _startPos + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 min = Camera.main.ScreenToWorldPoint(selectionBox.position - (Vector3)(selectionBox.sizeDelta / 2));
        Vector2 max = Camera.main.ScreenToWorldPoint(selectionBox.position + (Vector3)(selectionBox.sizeDelta / 2));
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        Physics2D.BoxCast(Camera.main.ScreenToWorldPoint(selectionBox.position), 
            max-min,0,Vector2.zero, PlayerFilter, results);
        foreach(RaycastHit2D hit in results)
        {
            ShipLogic battleController = hit.collider.GetComponent<ShipLogic>();
            
            if (!selectedShips.ShipList.Contains(battleController.gameObject))
            { 
                selectedShips.AddShip(battleController.gameObject);
                battleController.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
        }
    }
    private void CombatLeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                DeSelectShips();
                _drawSelectionBox = true;
                _startPos = _mousePos;
                ShipLogic shipBattleClicked = ShipRaycast();
                if (shipBattleClicked != null)
                {
                    selectedShips.AddShip(shipBattleClicked.gameObject);
                    shipBattleClicked.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                break;
            case InputActionPhase.Canceled:
                if (_drawSelectionBox)
                {
                    _drawSelectionBox = false;
                    ReleaseSelectionBox();
                }
                break;
        }
    }

    private void CombatRightClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if(selectedShips.Count > 0)
                    MoveSelectedShips(_mousePos);
                break;
        }
    }
    
    private void StoreLeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _startPos = _mousePos;
                if (UIRaycast())
                    return;
                ShipLogic shipClicked = ShipRaycast();
                if (shipClicked != null && selectedShips.Count <= 1)
                {
                    if (!selectedShips.ShipList.Contains(shipClicked.gameObject))
                    {
                        selectedShips.AddShip(shipClicked.gameObject);
                        shipClicked.GetComponent<SpriteRenderer>().color = Color.cyan;
                    }else if (selectedShips.Count == 1 && selectedShips.ShipList[0].gameObject.Equals(shipClicked.gameObject))
                    {
                        _drawSelectionBox = false;
                    }
                    else
                    {
                        selectedShips.RemoveShip(shipClicked.gameObject);
                        shipClicked.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                if (selectedShips.Count > 0 && shipClicked != null)
                {
                    _dragShips = true;
                    _startPos = _mousePos;
                    foreach (GameObject ship in selectedShips.ShipList)
                    {
                        var controller = ship.GetComponent<ShipLogic>();
                        controller.positionOffset = (Vector2) ship.transform.position - _projectedMousePos;
                    }
                }else if (selectedShips.Count > 0 && shipClicked == null)
                {
                    DeSelectShips();
                }else
                {
                    _drawSelectionBox = true;
                }
                break;
            case InputActionPhase.Canceled:
                if (_dragShips)
                {
                    _dragShips = false;
                    if(Vector2.Distance(_startPos,_mousePos) > 1)
                        DeSelectShips();
                }
                if (_drawSelectionBox)
                {
                    _drawSelectionBox = false;
                    ReleaseSelectionBox();
                }
                break;
        }
    }
    private void StoreRightClick(InputAction.CallbackContext context)
    {
        
    }
    
    private ShipLogic ShipRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(_projectedMousePos, Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask("PlayerShips","UI"));
        if (hit.collider != null)
        {
            ShipLogic shipBattle = hit.collider.gameObject.GetComponent<ShipLogic>();
            return shipBattle;
        }
        return null;
    }

    private bool UIRaycast()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = _mousePos;
        List<RaycastResult> hits = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, hits);
        if (hits.Count > 0)
            return true;
        return false;
    }
    public void DeSelectShips()
    {
        foreach(GameObject ship in selectedShips.ShipList)
        {
            if (ship != null)
            {
                ship.GetComponent<ShipLogic>().GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        selectedShips.ClearList();
    }

    public void DeselectShip(GameObject ship)
    {
        selectedShips.RemoveShip(ship);
    }
    public void MoveSelectedShips(Vector2 position)
    {
        foreach(GameObject ship in selectedShips.ShipList)
        {
            if (ship != null)
            {
                ship.GetComponent<ShipLogic>().MoveToPosition(Camera.main.ScreenToWorldPoint(position));
            }
        }
        _drawSelectionBox = false;
    }
    private void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
