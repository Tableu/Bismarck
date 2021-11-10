using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

//Selection box code: https://gamedevacademy.org/rts-unity-tutorial/
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private PlayerInputActions _playerInputActions;
    [SerializeField] private List<GameObject> _selectedShips;
    [SerializeField] private RectTransform selectionBox;
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

    private void EnableCombatInput()
    {
        _playerInputActions.Store.Disable();
        _playerInputActions.Combat.Enable();
        Time.timeScale = 1f;
    }
    private void EnableStoreInput()
    {
        _playerInputActions.Combat.Disable();
        _playerInputActions.Store.Enable();
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

        _playerInputActions.Store.LeftClick.started += StoreLeftClick;
        _playerInputActions.Store.LeftClick.performed += StoreLeftClick;
        _playerInputActions.Store.LeftClick.canceled += StoreLeftClick;
        
        _playerInputActions.Store.RightClick.started += StoreRightClick;
        _playerInputActions.Store.RightClick.performed += StoreRightClick;
        _playerInputActions.Store.RightClick.canceled += StoreRightClick;
        
        EnableCombatInput();
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
            foreach (GameObject ship in _selectedShips)
            {
                if (ship != null)
                {
                    ship.transform.position = _projectedMousePos;
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
                controller.Highlight();
                SelectShip(ship);
            }
        }
    }
    private void CombatLeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _selectedShips.Clear();
                _drawSelectionBox = true;
                SelectShips();
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
                if(_selectedShips.Count > 0)
                    MoveSelectedShips(_mousePos);
                break;
        }
    }

    private void StoreLeftClick(InputAction.CallbackContext context){
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _selectedShips.Clear();
                SelectShips();
                _dragShips = true;
                break;
            case InputActionPhase.Canceled:
                _dragShips = false;
                DeSelectShips();
                break;
        }
    }
    private void StoreRightClick(InputAction.CallbackContext context)
    {
        
    }
    private void SelectShips()
    {
        _startPos = _mousePos;
        RaycastHit2D hit = Physics2D.Raycast(_projectedMousePos, Vector2.zero, Mathf.Infinity,
            LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            ShipController ship = hit.collider.gameObject.GetComponent<ShipController>();
            if (ship != null)
            {
                ship.Highlight();
                SelectShip(ship.gameObject);
            }
        }
    }
    private void DeSelectShips()
    {
        foreach(GameObject ship in _selectedShips.ToList())
        {
            if (ship != null)
            {
                ship.GetComponent<ShipController>().DeHighlight();
            }
        }
        _selectedShips.Clear();
    }
    public void SelectShip(GameObject ship)
    {
        if(!_selectedShips.Contains(ship))
            _selectedShips.Add(ship);
    }

    public void DeselectShip(GameObject ship)
    {
        _selectedShips.Remove(ship);
    }
    public void SelectShip(List<GameObject> ships)
    {
        _selectedShips.AddRange(ships);
    }

    public void MoveSelectedShips(Vector2 position)
    {
        foreach(GameObject ship in _selectedShips.ToList())
        {
            if (ship != null)
            {
                ship.GetComponent<ShipController>().DeHighlight();
                ship.GetComponent<ShipController>().MoveToPosition(Camera.main.ScreenToWorldPoint(position));
            }
        }
        _drawSelectionBox = false;
        _selectedShips.Clear();
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
