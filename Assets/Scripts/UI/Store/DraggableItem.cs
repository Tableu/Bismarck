using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent ItemSelected;
    public UnityEvent ItemReleased;
    public string ItemName;
    private bool holding;
    private Vector2 originalPos;

    private void Start()
    {

    }

    private void Update()
    {
        if (holding)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            holding = true;
            originalPos = transform.position;
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
