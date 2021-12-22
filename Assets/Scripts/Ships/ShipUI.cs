using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShipUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ShipDictionary ShipDictionary;
    public ShipListScriptableObject selectedShips;
    private Vector2 _positionOffset;
    private bool _selected;

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectShip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RefreshPositionOffset();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + _positionOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
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
