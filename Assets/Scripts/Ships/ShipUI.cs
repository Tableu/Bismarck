using UnityEngine;
using UnityEngine.InputSystem;

public class ShipUI : MonoBehaviour
{
    public ShipListScriptableObject selectedShips;
    [SerializeField] private Vector2 _positionOffset;

    public void OnDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + _positionOffset;
    }

    public void RefreshPositionOffset()
    {
        _positionOffset = transform.position - Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void SelectShip()
    {
        RefreshPositionOffset();
        if (!selectedShips.ShipList.Contains(gameObject))
        {
            selectedShips.AddShip(gameObject);
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    public void DeselectShip()
    {
        if (selectedShips.ShipList.Contains(gameObject))
        {
            selectedShips.RemoveShip(gameObject);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
