using System;
using System.Collections.Generic;
using Systems.Modules;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     UI representation of the grid in ModulesInfo. Instantiates Module Slots and ModuleViews
///     in a Canvas Grid.
/// </summary>
public class ModuleGridView : MonoBehaviour
{
    public int UnitSize;
    public ModulesInfo ModulesInfo;
    public GameObject ModuleView;
    public GameObject ModuleSlot;
    public GraphicRaycaster GraphicRaycaster;
    private int _columnLength;
    private int _rowHeight;
    private bool _initialized;
    private List<GameObject> _moduleViews;
    private List<ModuleGridSlot> _currentGridSlots;
    private ModuleGridSlot[,] _slotGrid;

    private void Awake()
    {
        ModulesInfo.ModuleGridChanged += ModuleGridChanged;
        _moduleViews = new List<GameObject>();
    }

    public ModuleGridSlot GetGridSlot(int row, int column)
    {
        if (row >= 0 && row < _rowHeight &&
            column >= 0 && column < _columnLength)
        {
            return _slotGrid[row, column];
        }

        return null;
    }

    private void ModuleGridChanged()
    {
        if (!_initialized)
        {
            DrawSlots();
        }

        RefreshGrid();
    }

    private void DrawSlots()
    {
        _columnLength = ModulesInfo.ColumnLength;
        _rowHeight = ModulesInfo.RowHeight;
        _slotGrid = new ModuleGridSlot[_rowHeight, _columnLength];

        for (int r = 0; r < _rowHeight; r++)
        {
            for (int c = 0; c < _columnLength; c++)
            {
                GameObject slot = Instantiate(ModuleSlot, transform, false);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0) * UnitSize;
                ModuleGridSlot gridSlot = slot.GetComponent<ModuleGridSlot>();
                gridSlot.ModuleGridView = this;
                gridSlot.Position = new Vector2Int(c, r);
                _slotGrid[r, c] = gridSlot;
            }
        }

        _initialized = true;
    }

    private void RefreshGrid()
    {
        foreach (GameObject moduleView in _moduleViews)
        {
            Destroy(moduleView);
        }
        foreach (Module module in ModulesInfo.Modules)
        {
            if (module != null)
            {
                GameObject e = Instantiate(ModuleView, transform, false);
                _moduleViews.Add(e);
                ModuleView moduleView = e.GetComponent<ModuleView>();
                moduleView.Module = module;
                moduleView.GraphicRaycaster = GraphicRaycaster;
                moduleView.ModuleGridView = this;
                e.GetComponent<RectTransform>().anchoredPosition =
                    new Vector3(module.RootPosition.x, module.RootPosition.y, 0) * UnitSize;
            }
        }
    }
}