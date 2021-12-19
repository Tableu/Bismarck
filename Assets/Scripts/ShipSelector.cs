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
                    ShipBattleController battleController = ship.GetComponent<ShipBattleController>();
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
            ShipController battleController = hit.collider.GetComponent<ShipController>();
            
            if (!selectedShips.ShipList.Contains(battleController.gameObject))
            { 
                selectedShips.AddShip(battleController.gameObject);
                battleController.Highlight();
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
                ShipController shipBattleClicked = ShipRaycast();
                if (shipBattleClicked != null)
                {
                    selectedShips.AddShip(shipBattleClicked.gameObject);
                    shipBattleClicked.Highlight();
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
    
    private ShipBattleController ShipRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(_projectedMousePos, Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask("PlayerShips","UI"));
        if (hit.collider != null)
        {
            ShipBattleController shipBattle = hit.collider.gameObject.GetComponent<ShipBattleController>();
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
                ship.GetComponent<ShipBattleController>().DeHighlight();
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
                ship.GetComponent<ShipBattleController>().MoveToPosition(Camera.main.ScreenToWorldPoint(position));
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
