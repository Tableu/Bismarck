using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
///     A Canvas UI item that can be dragged (after clicking and holding)
/// </summary>
public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent ItemSelected;
    public UnityEvent ItemReleased;
    public RectTransform RectTransform;
    public string ItemName;
    private bool holding;
    private Vector2 originalPos;
    private Vector2 offset;

    private void Update()
    {
        if (holding)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RectTransform.position = mousePos + offset;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            holding = true;
            originalPos = RectTransform.position;
            offset = originalPos - (Vector2) Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            ItemSelected.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ItemReleased.Invoke();
            holding = false;
        }
    }

    public void ReturnToOriginalPosition()
    {
        RectTransform.position = originalPos;
    }
}
