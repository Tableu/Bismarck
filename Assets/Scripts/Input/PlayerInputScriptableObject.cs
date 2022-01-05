using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "PlayerInput/PlayerInput")]
public class PlayerInputScriptableObject : ScriptableObject
{
    public PlayerInputActions PlayerInputActions;
    public ShipListScriptableObject SelectedShips;

    public ContactFilter2D PlayerFilter; 
    private void OnEnable()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerFilter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("PlayerShips"),
            useLayerMask = true
        };
    }
    public bool UIRaycast(Vector2 position)
    {
        var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI"));
        if (hit)
        {
            return true;
        }
        return false;
    }

    public bool ShipRaycast(Vector2 position)
    {
        var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("PlayerShips"));
        if (hit)
        {
            return true;
        }
        return false;
    }
    
    public void DeSelectShips()
    {
        if (SelectedShips.Count <= 0)
        {
            return;
        }
        foreach(GameObject ship in SelectedShips.ShipList)
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
        SelectedShips.ClearList();
    }
}
