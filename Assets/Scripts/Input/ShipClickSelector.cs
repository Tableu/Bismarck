using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipClickSelector : MonoBehaviour
{
    public PlayerInputScriptableObject playerInput;
    public ShipList selectedShips;
    public UnityEvent SelectedShipsEvent;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private PlayerInputActions _playerInputActions;
    private Vector2 _startPos;

    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.UI.LeftClick.started += LeftClick;
        _playerInputActions.UI.LeftClick.canceled += LeftClick;
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.LeftClick.started -= LeftClick;
        _playerInputActions.UI.LeftClick.canceled -= LeftClick;
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        switch (context.phase)
        {
            case InputActionPhase.Started:
                _startPos = mousePos;
                if (!playerInput.UIRaycast(graphicRaycaster) && playerInput.ShipRaycast(mousePos))
                {
                    var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity,
                        LayerMask.GetMask("PlayerShips"));
                    if (hit)
                    {
                        selectedShips.AddShip(hit.transform.gameObject);
                        ShipUI shipUI = hit.transform.gameObject.GetComponent<ShipUI>();
                        if (shipUI != null)
                        {
                            shipUI.SelectShip();
                        }

                        SelectedShipsEvent.Invoke();
                    }
                }

                break;
            case InputActionPhase.Canceled:
                if ((mousePos - _startPos).sqrMagnitude < 0.5 && !playerInput.ShipRaycast(mousePos) &&
                    !playerInput.UIRaycast(graphicRaycaster))
                {
                    playerInput.DeSelectShips();
                    SelectedShipsEvent.Invoke();
                }

                break;
        }
    }
}