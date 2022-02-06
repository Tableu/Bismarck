using System;
using Systems.Modules;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     UI representation of the grid in ModulesInfo. Instantiates Module Slots and ModuleViews
///     in a Canvas Grid.
/// </summary>
public class ModuleGridView : MonoBehaviour
{
    private int _columnLength;
    private int _rowHeight;
    public int UnitSize;
    public ModulesInfo ModulesInfo;
    public GameObject ModuleView;
    public GameObject ModuleSlot;
    public GraphicRaycaster GraphicRaycaster;

    private void Awake()
    {
        ModulesInfo.ModulesInfoInitialized += ModuleInfoInitialized;
    }

    private void ModuleInfoInitialized(object sender, EventArgs e)
    {
        DrawModuleGrid();
    }

    private void DrawModuleGrid()
    {
        _columnLength = ModulesInfo.ColumnLength;
        _rowHeight = ModulesInfo.RowHeight;

        for (int r = 0; r < _rowHeight; r++)
        {
            for (int c = 0; c < _columnLength; c++)
            {
                GameObject slot = Instantiate(ModuleSlot, transform, false);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0) * UnitSize;
                slot.GetComponent<ModuleGridSlot>().moduleGridView = this;
                slot.GetComponent<ModuleGridSlot>().Position = new Vector2Int(c, r);
            }
        }

        foreach (Module module in ModulesInfo.Modules)
        {
            if (module != null)
            {
                GameObject e = Instantiate(ModuleView, transform, false);
                ModuleView moduleView = e.GetComponent<ModuleView>();
                moduleView.Module = module;
                moduleView.GraphicRaycaster = GraphicRaycaster;
                e.GetComponent<RectTransform>().anchoredPosition =
                    new Vector3(module.RootPosition.x, module.RootPosition.y, 0) * UnitSize;
            }
        }
    }
}