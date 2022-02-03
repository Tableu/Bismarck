using System.Collections;
using System.Collections.Generic;
using Systems.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModuleGridItem : MonoBehaviour
{
    public Module Module;
    [SerializeField] private Image _image;
    [SerializeField] private DraggableItem _draggableItem;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    
    void Start()
    {
        if (Module != null)
        {
            _image.sprite = Module.Data.GridSprite;
        }
    }

    public void AddModule()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> hits = new List<RaycastResult>();
        _graphicRaycaster.Raycast(eventData, hits);
        if (hits[1].gameObject.CompareTag("EmptyModuleSlot"))
        {
            ModuleGridSlot moduleGridSlot = hits[1].gameObject.GetComponentInParent<ModuleGridSlot>();
            if (moduleGridSlot != null)
            {
                moduleGridSlot.ModuleGrid.AddModule(Module, moduleGridSlot.SlotPosition);
                transform.SetParent(moduleGridSlot.transform.parent);
                GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    Module.PivotPosition.x * moduleGridSlot.ModuleGrid.UnitSize,
                Module.PivotPosition.y * moduleGridSlot.ModuleGrid.UnitSize);
            }
        }
    }
}
