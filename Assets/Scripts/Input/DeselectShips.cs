using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DeselectShips : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public ShipListScriptableObject selectedShips;
    public ShipListScriptableObject playerShips;
    public PlayerInputScriptableObject playerInput;
    public UnityEvent SelectedShipsEvent;
    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
    }
    void Start()
    {
        _playerInputActions.UI.LeftClick.canceled += LeftClick;
    }
    
    void Update()
    {
        
    }
    
    private void LeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        switch (context.phase)
        {
            case InputActionPhase.Canceled:
                if (!playerInput.ShipRaycast(mousePos))
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
        foreach(GameObject ship in selectedShips.ShipList)
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
    
    private void OnDestroy()
    {
        _playerInputActions.UI.LeftClick.canceled -= LeftClick;
    }
}
