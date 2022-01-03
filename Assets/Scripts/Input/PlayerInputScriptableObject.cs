using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "PlayerInput/PlayerInput")]
public class PlayerInputScriptableObject : ScriptableObject
{
    public PlayerInputActions PlayerInputActions;

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
    public bool UIRaycast(Vector2 position, GraphicRaycaster graphicRaycaster)
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        List<RaycastResult> hits = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, hits);
        if (hits.Count > 0)
            return true;
        return false;
    }

    public bool ShipRaycast(Vector2 position)
    {
        var hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("PlayerShips", "UI"));
        if (hit)
        {
            return true;
        }
        return false;
    }
}
