using Systems.Modules;
using UnityEngine;

public class ModuleGridView : MonoBehaviour
{
    private int _columnLength;
    private int _rowHeight;
    public int UnitSize;
    public ModulesInfo ModulesInfo;
    public GameObject ModuleView;
    public GameObject ModuleSlot;
    
    void Start()
    {
        _columnLength = ModulesInfo.ColumnLength;
        _rowHeight = ModulesInfo.RowHeight;
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
        for(int r = 0; r < _rowHeight; r++)
        {
            for(int c = 0; c < _columnLength; c++)
            {
                Coordinates pos = new Coordinates
                {
                    x = c,
                    y = r
                };
                if (ModulesInfo.GetModule(pos) == null)
                {
                    GameObject empty = Instantiate(ModuleSlot, transform, false);
                    empty.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0)*UnitSize;
                    empty.GetComponent<ModuleGridSlot>().moduleGridView = this;
                    empty.GetComponent<ModuleGridSlot>().SlotPosition = new Coordinates
                    {
                        x = c,
                        y = r
                    };
                }
                
            }
        }
    }
}