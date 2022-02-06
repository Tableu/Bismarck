using System.Collections.Generic;
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
public class ModuleView : DraggableItem
{
    public Module Module;
    public GraphicRaycaster GraphicRaycaster;
    [SerializeField] private Image _image;

    void Start()
    {
        if (Module != null)
        {
            _image.sprite = Module.Data.GridSprite;
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
        if (hits[1].gameObject.CompareTag("EmptyModuleSlot"))
        {
            ModuleGridSlot moduleGridSlot = hits[1].gameObject.GetComponentInParent<ModuleGridSlot>();
            if (moduleGridSlot != null)
            {
                if (moduleGridSlot.moduleGridView.ModulesInfo.AddModule(Module, moduleGridSlot.Position))
                {
                    return;
                }
            }
        }

        ReturnToOriginalPosition();
    }
}
