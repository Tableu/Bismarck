using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveShips : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public ShipListScriptableObject selectedShips;
    public PlayerInputScriptableObject playerInput;
    [SerializeField]private Vector2 _projectedMousePos;
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
                if(selectedShips.Count > 0)
                    MoveSelectedShips(_projectedMousePos);
                break;
        }
    }
    public void MoveSelectedShips(Vector2 position)
    {
        foreach(GameObject ship in selectedShips.ShipList)
        {
            if (ship != null)
            {
                ship.GetComponent<ShipLogic>().MoveToPosition(position);
            }
        }
    }
}
