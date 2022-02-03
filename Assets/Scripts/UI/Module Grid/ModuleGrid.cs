using Systems.Modules;
using Ships.DataManagement;
using UnityEngine;

public class ModuleGrid : MonoBehaviour
{
    private int _columnLength;
    private int _rowHeight;
    public int UnitSize;
    public ModulesInfo modulesInfo;
    private Module[,] _grid;
    public GameObject GridItem;
    public GameObject EmptyGridSpace;
    // Start is called before the first frame update
    void Start()
    {
        _grid = modulesInfo.Grid;
        _columnLength = _grid.GetLength(0);
        _rowHeight = _grid.GetLength(1);
        for(int r = 0; r < _rowHeight; r++)
        {
            for(int c = 0; c < _columnLength; c++)
            {
                var module = _grid[r, c];
                if (module != null)
                {
                    if (module.PivotPosition.x == c && module.PivotPosition.y == r)
                    {
                        GameObject e = Instantiate(GridItem, transform, false);
                        e.GetComponent<ModuleGridItem>().Module = module;
                        e.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0)*UnitSize;
                    }
                }
                else
                {
                    GameObject empty = Instantiate(EmptyGridSpace, transform, false);
                    empty.GetComponent<RectTransform>().anchoredPosition = new Vector3(c, r, 0)*UnitSize;
                    empty.GetComponent<ModuleGridSlot>().ModuleGrid = this;
                    empty.GetComponent<ModuleGridSlot>().SlotPosition = new Coordinates()
                    {
                        x = c,
                        y = r
                    };
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddModule(Module module, Coordinates coords)
    {
        foreach (Coordinates gridPos in module.Data.GridPositions)
        {
            if (_grid[gridPos.y+coords.y, gridPos.x+coords.x] != null)
            {
                return;
            }
        }
        modulesInfo.AddModule(module, coords.y, coords.x);
    }

    public void RemoveModule(Module module)
    {
        modulesInfo.RemoveModule(module);
    }
}