using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//Allows the player to click and hold on a GameObject to drag it around the screen.
//When the player releases the mouse button the GameObject is placed at that position.
public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent ItemSelected;
    public UnityEvent ItemReleased;
    public string ItemName;
    private Vector2 originalPos;
    private Vector2 offset;
    private bool holding;

    private void Update()
    {
        if (holding)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(pos.x, pos.y, 0) + (Vector3)offset;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            holding = true;
            originalPos = transform.position;
            offset = originalPos - (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            ItemSelected.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ItemReleased.Invoke();
            //transform.position = new Vector3(originalPos.x, originalPos.y, 0);
            holding = false;
        }
    }
}