using Modules;
using Ships.DataManagement;
using UnityEngine;

public class ModuleGrid : MonoBehaviour
{
    public int ColumnLength;
    public int RowHeight;
    public ShipData ShipData;
    public GameObject[,] Grid;
    public GameObject Element;
    public RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        ModuleData[,] moduleGrid = ShipData.ModuleGrid.Grid;
        ColumnLength = moduleGrid.GetLength(0);
        RowHeight = moduleGrid.GetLength(1);
        Grid = new GameObject[RowHeight, ColumnLength];
        for(int r = 0; r < RowHeight; r++)
        {
            for(int c = 0; c < ColumnLength; c++)
            {
                if (moduleGrid[r, c] != null)
                {
                    GameObject e = Instantiate(moduleGrid[r, c].GridSprite, transform, false);
                    e.GetComponent<RectTransform>().anchoredPosition = new Vector3(c * 100, r * 100, 0);
                    foreach (Coordinates coords in moduleGrid[r, c].GridPositions)
                    {
                        Grid[coords.y, coords.x] = e;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}