using Systems.Modules;
using UnityEngine;

public class ModuleGridView : MonoBehaviour
{
    private int _columnLength;
    private int _rowHeight;
    public int UnitSize;
    public ModulesInfo ModulesInfo;
    public GameObject ModuleView;
    public GameObject EmptyGridSpace;
    // Start is called before the first frame update
    void Start()
    {
        _columnLength = ModulesInfo.ColumnLength;
        _rowHeight = ModulesInfo.RowHeight;
        for(int r = 0; r < _rowHeight; r++)
        {
            for(int c = 0; c < _columnLength; c++)
            {
                var module = ModulesInfo.GetModule(new Coordinates()
                {
                    x = c,
                    y = r
                });
                if (module != null)
                {
                    if (module.RootPosition.x == c && module.RootPosition.y == r)
                    {
                        GameObject e = Instantiate(ModuleView, transform, false);
                        e.GetComponent<ModuleView>().ModuleData = module.Data;
                        e.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0)*UnitSize;
                    }
                }
                else
                {
                    GameObject empty = Instantiate(EmptyGridSpace, transform, false);
                    empty.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0)*UnitSize;
                    empty.GetComponent<ModuleGridSlot>().moduleGridView = this;
                    empty.GetComponent<ModuleGridSlot>().SlotPosition = new Coordinates()
                    {
                        x = c,
                        y = r
                    };
                }
            }
        }
    }
}