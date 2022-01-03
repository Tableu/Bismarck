using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipClickSelector : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public PlayerInputScriptableObject playerInput;
    public UnityEvent SelectedShipsEvent;
    [SerializeField] private GraphicRaycaster graphicRaycaster;

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
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (!playerInput.UIRaycast(mousePos, graphicRaycaster) && playerInput.ShipRaycast(mousePos))
                {
                    var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("PlayerShips"));
                    if (hit)
                    {
                        ShipUI shipUI = hit.collider.gameObject.GetComponent<ShipUI>();
                        if (shipUI != null)
                        {
                            shipUI.SelectShip();
                        }

                        SelectedShipsEvent.Invoke();
                    }
                }
                break;
        }
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.LeftClick.started -= LeftClick;
    }
}
