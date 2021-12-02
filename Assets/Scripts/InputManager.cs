using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Selection box code: https://gamedevacademy.org/rts-unity-tutorial/
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private PlayerInputActions _playerInputActions;
    public List<GameObject> selectedShips;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private Vector2 _startPos;
    private Vector2 _mousePos;
    private Vector2 _projectedMousePos;
    private bool _drawSelectionBox = false;
    private bool _dragShips = false;

    public static InputManager Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        _instance = this;
        _playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        _playerInputActions.Enable();
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
        
        EnableStoreInput();
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
                    ShipController controller = ship.GetComponent<ShipController>();
                    ship.transform.position = _projectedMousePos + controller.positionOffset;
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
        foreach(GameObject ship in ShipManager.Instance.Ships(gameObject).ToList())
        {
            ShipController controller = ship.GetComponent<ShipController>();
            Vector3 shipPos = ship.transform.position;
        
            if(shipPos.x > min.x && shipPos.x < max.x && shipPos.y > min.y && shipPos.y < max.y)
            {
                if (!selectedShips.Contains(ship))
                {
                    selectedShips.Add(ship);
                    controller.Highlight();
                }
            }
        }
    }
    private void CombatLeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                selectedShips.Clear();
                _drawSelectionBox = true;
                _startPos = _mousePos;
                ShipController shipClicked = ShipRaycast();
                if (shipClicked != null)
                {
                    selectedShips.Add(shipClicked.gameObject);
                    shipClicked.Highlight();
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
                        var controller = ship.GetComponent<ShipController>();
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
                UpdateRepairCost();
                UpdateSellCost();
                break;
        }
    }
    private void StoreRightClick(InputAction.CallbackContext context)
    {
        
    }
    private ShipController ShipRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(_projectedMousePos, Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask("Player","UI"));
        if (hit.collider != null)
        {
            ShipController ship = hit.collider.gameObject.GetComponent<ShipController>();
            return ship;
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
        if (StoreManager.Instance == null)
            return;
        foreach(GameObject ship in selectedShips.ToList())
        {
            if (ship != null)
            {
                ship.GetComponent<ShipController>().DeHighlight();
            }
        }
        selectedShips.Clear();
        StoreManager.Instance.UpdateRepairText(0);
        StoreManager.Instance.UpdateSellText(0);
    }

    public void DeselectShip(GameObject ship)
    {
        selectedShips.Remove(ship);
    }
    public void SelectShip(List<GameObject> ships)
    {
        selectedShips.AddRange(ships);
    }

    public void MoveSelectedShips(Vector2 position)
    {
        foreach(GameObject ship in selectedShips.ToList())
        {
            if (ship != null)
            {
                ship.GetComponent<ShipController>().DeHighlight();
                ship.GetComponent<ShipController>().MoveToPosition(Camera.main.ScreenToWorldPoint(position));
            }
        }
        _drawSelectionBox = false;
        selectedShips.Clear();
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

    private void UpdateSellCost()
    {
        if (StoreManager.Instance == null)
            return;
        int total = 0;
        foreach (GameObject ship in selectedShips)
        {
            if (ship != null)
            {
                var controller = ship.GetComponent<ShipController>();
                total += controller.SellCost();
            }
        }
        StoreManager.Instance.UpdateSellText(total);
    }
    private void UpdateRepairCost()
    {
        if (StoreManager.Instance == null)
            return;
        int total = 0;
        foreach (GameObject ship in selectedShips)
        {
            if (ship != null)
            {
                var controller = ship.GetComponent<ShipController>();
                total += controller.RepairCost();
            }
        }
        StoreManager.Instance.UpdateRepairText(total);
    }
}
