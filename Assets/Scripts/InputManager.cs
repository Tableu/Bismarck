using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

//Selection box code: https://gamedevacademy.org/rts-unity-tutorial/
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private PlayerInputActions _playerInputActions;
    [SerializeField] private List<GameObject> _selectedShips;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Camera mainCamera;
    private Vector2 _startPos;
    private Vector2 _mousePos;
    private Vector2 _projectedMousePos;
    private bool _drawSelectionBox = false;

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
    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.UI.Pause.started += Pause;
        _playerInputActions.UI.Click.started += OnClick;
        _playerInputActions.UI.Click.performed += OnClick;
        _playerInputActions.UI.Click.canceled += OnClick;
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = _playerInputActions.UI.Point.ReadValue<Vector2>();
        _projectedMousePos = mainCamera.ScreenToWorldPoint(_mousePos);
        if (_drawSelectionBox)
        {
            UpdateSelectionBox();
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
        Vector2 min = mainCamera.ScreenToWorldPoint(selectionBox.position - (Vector3)(selectionBox.sizeDelta / 2));
        Vector2 max = mainCamera.ScreenToWorldPoint(selectionBox.position + (Vector3)(selectionBox.sizeDelta / 2));
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
    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_selectedShips.Count <= 0)
            {
                _startPos = _mousePos;
                _drawSelectionBox = true;
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
            else
            {
                MoveSelectedShips(_mousePos);
            }
        }else if (context.canceled)
        {
            if (_drawSelectionBox)
            {
                _drawSelectionBox = false;
                ReleaseSelectionBox();
            }
        }
    }
    public void SelectShip(GameObject ship)
    {
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
                ship.GetComponent<ShipController>().Highlight();
                ship.GetComponent<ShipController>().MoveToPosition(mainCamera.ScreenToWorldPoint(position));
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
