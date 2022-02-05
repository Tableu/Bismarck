using System.Collections.Generic;
using Systems.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModuleView : MonoBehaviour
{
    public ModuleData ModuleData;
    [SerializeField] private Image _image;
    [SerializeField] private DraggableItem _draggableItem;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    
    void Start()
    {
        if (ModuleData != null)
        {
            _image.sprite = ModuleData.GridSprite;
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
                moduleGridSlot.moduleGridView.ModulesInfo.AddModule(ModuleData, moduleGridSlot.SlotPosition);
                Module module = moduleGridSlot.moduleGridView.ModulesInfo.GetModule(moduleGridSlot.SlotPosition);
                transform.SetParent(moduleGridSlot.transform.parent);
                GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    module.RootPosition.x * moduleGridSlot.moduleGridView.UnitSize,
                    module.RootPosition.y * moduleGridSlot.moduleGridView.UnitSize);
            }
        }
    }
}
