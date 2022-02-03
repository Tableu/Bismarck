using UnityEngine;
using UnityEngine.InputSystem;

public class MoveShips : MonoBehaviour
{
    public ShipList selectedShips;
    public PlayerInputScriptableObject playerInput;
    [SerializeField] private Vector2 _projectedMousePos;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.UI.RightClick.started += RightClick;
        _playerInputActions.UI.RightClick.performed += RightClick;
        _playerInputActions.UI.RightClick.canceled += RightClick;
    }

    // Update is called once per frame
    void Update()
    {
        _projectedMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (selectedShips.Count > 0)
                {
                    MoveSelectedShips(_projectedMousePos);
                }
                break;
        }
    }

    public void MoveSelectedShips(Vector2 position)
    {
        foreach (GameObject ship in selectedShips.Ships)
        {
            if (ship != null)
            {
                ship.GetComponent<ShipLogic>().MoveToPosition(position);
            }
        }
    }
}
