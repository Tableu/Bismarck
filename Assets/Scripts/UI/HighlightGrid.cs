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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
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

        IGridItem item = hits[0].gameObject.GetComponent<IGridItem>();
        List<Vector2Int> positions = null;
        if (item != null)
        {
            positions = item.GetItemPositions();
        }

        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject.CompareTag("EmptyModuleSlot"))
            {
                IGridSlot gridSlot = hit.gameObject.GetComponent<IGridSlot>();
                if (gridSlot != null)
                {
                    if (_currentSlot != null)
                    {
                        _currentSlot.Exit();
                    }

                    gridSlot.Enter(positions);
                    _currentSlot = gridSlot;
                    return;
                }
            }
        }

        if (_currentSlot != null)
        {
            _currentSlot.Exit();
            _currentSlot = null;
        }
    }
}