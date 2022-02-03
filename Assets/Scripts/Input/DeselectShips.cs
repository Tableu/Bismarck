using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DeselectShips : MonoBehaviour
{
    public ShipList selectedShips;
    public ShipList playerShips;
    public PlayerInputScriptableObject playerInput;
    public UnityEvent SelectedShipsEvent;
    private PlayerInputActions _playerInputActions;
    private Vector2 _startPos;

    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
    }

    void Start()
    {
        _playerInputActions.UI.LeftClick.started += LeftClick;
        _playerInputActions.UI.LeftClick.canceled += LeftClick;
    }

    void Update()
    {
    }

    private void OnDestroy()
    {
        _playerInputActions.UI.LeftClick.canceled -= LeftClick;
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _startPos = mousePos;
                break;
            case InputActionPhase.Canceled:
                if (!playerInput.ShipRaycast(mousePos) && (mousePos - _startPos).sqrMagnitude < 0.5)
                {
                    DeSelectShips();
                }

                break;
        }
    }

    private void DeSelectShips()
    {
        if (selectedShips.Count <= 0)
        {
            return;
        }

        foreach (GameObject ship in selectedShips.Ships)
        {
            if (ship != null)
            {
                ShipUI shipUI = ship.GetComponent<ShipUI>();
                if (shipUI != null)
                {
                    shipUI.DeselectShip();
                }
            }
        }

        selectedShips.ClearList();
        SelectedShipsEvent.Invoke();
    }
}
