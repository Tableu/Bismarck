using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragShips : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public PlayerInputScriptableObject playerInput;
    public ShipListScriptableObject selectedShips;
    public GraphicRaycaster GraphicRaycaster;
    private static ContactFilter2D PlayerFilter;
    private bool _dragShips = false;
    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
        PlayerFilter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("PlayerShips"),
            useLayerMask = true
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.UI.LeftClick.started += LeftClick;
        _playerInputActions.UI.LeftClick.performed += LeftClick;
        _playerInputActions.UI.LeftClick.canceled += LeftClick;
    }

    // Update is called once per frame
    void Update()
    {
        if (_dragShips)
        {
            foreach (GameObject ship in selectedShips.ShipList)
            {
                if (ship == null)
                    continue;
                ShipUI shipUI = ship.GetComponent<ShipUI>();
                if (shipUI != null)
                {
                    shipUI.OnDrag();
                }
            }
        }
    }
    private void LeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if (!playerInput.UIRaycast(GraphicRaycaster) && playerInput.ShipRaycast(mousePos))
                {
                    foreach (GameObject ship in selectedShips.ShipList)
                    {
                        if (ship != null)
                        {
                            ShipUI shipUI = ship.GetComponent<ShipUI>();
                            if (shipUI != null)
                            {
                                shipUI.RefreshPositionOffset();
                            }
                        }
                    }
                    _dragShips = true;
                }
                break;
            case InputActionPhase.Canceled:
                _dragShips = false;
                break;
        }
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.LeftClick.started -= LeftClick;
        _playerInputActions.UI.LeftClick.performed -= LeftClick;
        _playerInputActions.UI.LeftClick.canceled -= LeftClick;
    }
}
