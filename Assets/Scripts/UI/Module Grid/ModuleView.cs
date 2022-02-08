using System.Collections.Generic;
using System.Linq;
using Systems.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
///     A drag-and-drop UI representation of a Module in a Canvas Grid.
///     Attempts to insert itself into the grid when dropped. If this fails it will snap back
///     to the previous position.
/// </summary>
public class ModuleView : DraggableItem, IGridItem
{
    public Module Module;
    public ModuleGridView ModuleGridView;
    public GraphicRaycaster GraphicRaycaster;
    [SerializeField] private Image _image;
    private Vector2Int _offset;

    private void Start()
    {
        if (Module != null && Module.Data != null)
        {
            _image.sprite = Module.Data.GridSprite;
            RectTransform.sizeDelta = ModuleGridView.UnitSize * Module.Data.Dimensions;
        }
    }

    public void AddModule()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
        
        List<RaycastResult> hits = new List<RaycastResult>();
        GraphicRaycaster.Raycast(eventData, hits);
        if (hits.Count > 1 && hits[1].gameObject.CompareTag("EmptyModuleSlot"))
        {
            ModuleGridSlot moduleGridSlot = hits[1].gameObject.GetComponentInParent<ModuleGridSlot>();
            if (moduleGridSlot != null)
            {
                if (moduleGridSlot.ModuleGridView.ModulesInfo.AddModule(Module, moduleGridSlot.Position + _offset))
                {
                    return;
                }
            }
        }

        ReturnToOriginalPosition();
    }

    public List<Vector2Int> GetItemPositions(Vector2Int clickPosition)
    {
        _offset = Module.RootPosition - clickPosition;
        var positionList = Module.Data.GridPositions.ToList();
        for (var index = 0; index < positionList.Count; index++)
        {
            positionList[index] = new Vector2Int(positionList[index].x + _offset.x, positionList[index].y + _offset.y);
        }

        return positionList;
    }
}
