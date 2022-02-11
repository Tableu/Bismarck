using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
///     Highlights grid spaces underneath the current grid item (either when dragged or placed
/// </summary>
public class HighlightGrid : MonoBehaviour
{
    public GraphicRaycaster GraphicRaycaster;

    private IGridSlot _currentSlot;
    private List<Vector2Int> _positions;

    private void Update()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> hits = new List<RaycastResult>();
        GraphicRaycaster.Raycast(eventData, hits);
        if (hits.Count <= 0)
        {
            return;
        }

        IGridItem item = !Mouse.current.leftButton.isPressed ? hits[0].gameObject.GetComponent<IGridItem>() : null;

        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject.CompareTag("EmptyModuleSlot"))
            {
                IGridSlot gridSlot = hit.gameObject.GetComponent<IGridSlot>();
                if (gridSlot != null)
                {
                    if (item != null)
                    {
                        _positions = item.GetItemPositions(gridSlot.GetPosition());
                    }
                    else if (!Mouse.current.leftButton.isPressed)
                    {
                        _positions = null;
                    }

                    if (_currentSlot != null)
                    {
                        _currentSlot.DeHighlight();
                    }

                    gridSlot.Highlight(_positions);
                    _currentSlot = gridSlot;
                    return;
                }
            }
        }

        if (_currentSlot != null)
        {
            _currentSlot.DeHighlight();
            _currentSlot = null;
        }
    }
}