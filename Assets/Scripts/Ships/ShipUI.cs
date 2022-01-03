using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShipUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public ShipDictionary ShipDictionary;
    public ShipListScriptableObject selectedShips;
    public UnityEvent ShipClicked;
    public UnityEvent ShipDragged;
    [SerializeField] private Vector2 _positionOffset;
    private bool _selected;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectShip();
            RefreshPositionOffset();
            ShipClicked.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + _positionOffset;
        ShipDragged.Invoke();
    }

    public void RefreshPositionOffset()
    {
        _positionOffset = transform.position - Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void SelectShip()
    {
        if (!selectedShips.ShipList.Contains(gameObject))
        {
            selectedShips.AddShip(gameObject);
            _selected = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    public void DeselectShip()
    {
        if (selectedShips.ShipList.Contains(gameObject))
        {
            selectedShips.RemoveShip(gameObject);
            _selected = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
