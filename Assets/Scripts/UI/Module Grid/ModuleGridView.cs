using Systems.Modules;
using Systems.Save;
using UnityEngine;

public class ModuleGridView : MonoBehaviour
{
    private int _columnLength;
    private int _rowHeight;
    public int UnitSize;
    public SaveManager SaveManager;
    public ModulesInfo ModulesInfo;
    public GameObject ModuleView;
    public GameObject ModuleSlot;
    
    void Start()
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
                slot.GetComponent<ModuleGridSlot>().SlotPosition = new Vector2Int(c, r);
            }
        }

        foreach (Module module in ModulesInfo.Modules)
        {
            if (module != null)
            {
                GameObject e = Instantiate(ModuleView, transform, false);
                e.GetComponent<ModuleView>().ModuleData = module.Data;
                e.GetComponent<RectTransform>().anchoredPosition =
                    new Vector3(module.RootPosition.x, module.RootPosition.y, 0) * UnitSize;
            }
        }
    }
}