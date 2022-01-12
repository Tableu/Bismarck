using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Selection box code: https://gamedevacademy.org/rts-unity-tutorial/
public class ShipBoxSelector : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public ShipListScriptableObject selectedShips;
    public ShipListScriptableObject playerShips;
    public PlayerInputScriptableObject playerInput;
    public UnityEvent SelectedShipsEvent;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private Vector2 _startPos;
    private Vector2 _projectedMousePos;
    private bool _drawSelectionBox = false;

    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
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
        _playerInputActions.UI.LeftClick.started += LeftClick;
        _playerInputActions.UI.LeftClick.canceled += LeftClick;
    }

    // Update is called once per frame
    void Update()
    {
        _projectedMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

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
        Vector3 min = selectionBox.position - (Vector3)(selectionBox.sizeDelta / 2);
        Vector3 max = selectionBox.position + (Vector3)(selectionBox.sizeDelta / 2);
        
        playerInput.DeSelectShips();
        foreach(GameObject ship in playerShips.ShipList)
        {
            if (ship == null)
                continue;
            Vector3 position = ship.transform.position;
            if (position.x < max.x && position.x > min.x && 
                position.y < max.y && position.y > min.y)
            {
                selectedShips.AddShip(ship);
                ShipUI shipUI = ship.GetComponent<ShipUI>();
                if (shipUI != null)
                {
                    shipUI.SelectShip();
                }
            }
        }
        
        SelectedShipsEvent.Invoke();
    }
    
    private void LeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (!playerInput.UIRaycast(graphicRaycaster) && !playerInput.ShipRaycast(_projectedMousePos))
                {
                    _drawSelectionBox = true;
                    _startPos = _projectedMousePos;
                }
                break;
            case InputActionPhase.Canceled:
                if (_drawSelectionBox && (_projectedMousePos-_startPos).sqrMagnitude >= 0.5)
                {
                    ReleaseSelectionBox();
                }
                _drawSelectionBox = false;
                break;
        }
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.LeftClick.started -= LeftClick;
        _playerInputActions.UI.LeftClick.canceled -= LeftClick;
    }
}
