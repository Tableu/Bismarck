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
    [SerializeField] private List<GameObject> _selectedShips;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject _grid;
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
        
        _grid = FindObjectOfType<GridLayoutGroup>().gameObject;
        if (_grid != null)
        {
            _grid.SetActive(false);
        }
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
        float width = _projectedMousePos.x - _startPos.x;
        float height = _projectedMousePos.y - _startPos.y;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.position = _startPos + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 min = selectionBox.position - (Vector3)(selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.position + (Vector3)(selectionBox.sizeDelta / 2);
        foreach(GameObject ship in ShipManager.Instance.PlayerShips.ToList())
        {
            ShipController controller = ship.GetComponent<ShipController>();
            Vector3 shipPos = ship.transform.position;
        
            if(shipPos.x > min.x && shipPos.x < max.x && shipPos.y > min.y && shipPos.y < max.y)
            {
                controller.OnPointerClick(new PointerEventData(null));
                //SelectShip(ship);
            }
        }
    }
    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _startPos = _projectedMousePos;
            _drawSelectionBox = true;
        }else if (context.canceled)
        {
            _drawSelectionBox = false;
            ReleaseSelectionBox();
        }
    }
    public void SelectShip(GameObject ship)
    {
        _grid.SetActive(true);
        _selectedShips.Add(ship);
    }

    public void DeselectShip(GameObject ship)
    {
        _selectedShips.Remove(ship);
    }
    public void SelectShip(List<GameObject> ships)
    {
        _grid.SetActive(true);
        _selectedShips.AddRange(ships);
    }

    public void GridItemSelected(GameObject item)
    {
        foreach(GameObject ship in _selectedShips.ToList())
        {
            if(ship != null)
                ship.GetComponent<ShipController>().OnPointerClick(new PointerEventData(null));
        }
        //_selectedShip.GetComponent<ShipController>().Move(item.transform.position);
        //item.GetComponent<Button>().enabled = false;
        _grid.SetActive(false);
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
