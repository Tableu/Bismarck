using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StoreInputManager : MonoBehaviour
{
    private static StoreInputManager _instance;
    private PlayerInputActions _playerInputActions;
    public List<GameObject> selectedShips;
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

    public static StoreInputManager Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _playerInputActions = new PlayerInputActions();
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
        _playerInputActions.UI.LeftClick.started += StoreLeftClick;
        _playerInputActions.UI.LeftClick.performed += StoreLeftClick;
        _playerInputActions.UI.LeftClick.canceled += StoreLeftClick;
        
        _playerInputActions.UI.RightClick.started += StoreRightClick;
        _playerInputActions.UI.RightClick.performed += StoreRightClick;
        _playerInputActions.UI.RightClick.canceled += StoreRightClick;
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
            foreach (GameObject ship in selectedShips)
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
            
            if (!selectedShips.Contains(battleController.gameObject))
            { 
                selectedShips.Add(battleController.gameObject);
                battleController.Highlight();
            }
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
                ShipController shipClicked = ShipRaycast();
                if (shipClicked != null && selectedShips.Count <= 1)
                {
                    if (!selectedShips.Contains(shipClicked.gameObject))
                    {
                        selectedShips.Add(shipClicked.gameObject);
                        shipClicked.Highlight();
                    }else if (selectedShips.Count == 1 && selectedShips[0].gameObject.Equals(shipClicked.gameObject))
                    {
                        _drawSelectionBox = false;
                    }
                    else
                    {
                        selectedShips.Remove(shipClicked.gameObject);
                        shipClicked.DeHighlight();
                    }
                }
                if (selectedShips.Count > 0 && shipClicked != null)
                {
                    _dragShips = true;
                    _startPos = _mousePos;
                    foreach (GameObject ship in selectedShips)
                    {
                        var controller = ship.GetComponent<ShipBattleController>();
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
        foreach(GameObject ship in selectedShips.ToList())
        {
            if (ship != null)
            {
                ship.GetComponent<ShipBattleController>().DeHighlight();
            }
        }
        selectedShips.Clear();
    }

    public void DeselectShip(GameObject ship)
    {
        selectedShips.Remove(ship);
    }
    public void SelectShip(List<GameObject> ships)
    {
        selectedShips.AddRange(ships);
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
