using UnityEngine;
using UnityEngine.InputSystem;

public class ShipUI : MonoBehaviour
{
    [SerializeField] private Vector2 _positionOffset;

    public void OnDrag()
    {
        transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) +
                             _positionOffset;
    }

    public void RefreshPositionOffset()
    {
        _positionOffset = transform.position - Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void SelectShip()
    {
        RefreshPositionOffset();
        gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    public void DeselectShip()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}