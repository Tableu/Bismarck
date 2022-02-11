using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "PlayerInput/PlayerInput")]
public class PlayerInputScriptableObject : ScriptableObject
{

    public ContactFilter2D PlayerFilter;
    public PlayerInputActions PlayerInputActions;

    private void OnEnable()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.Disable();
    }

    public bool UIRaycast(GraphicRaycaster graphicRaycaster)
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> hits = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, hits);
        if (hits.Count > 0)
        {
            return true;
        }
        return false;
    }

    public bool ShipRaycast(Vector2 position)
    {
        var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("PlayerShips"));
        return hit;
    }

    public void DeSelectShips()
    {

    }
}
