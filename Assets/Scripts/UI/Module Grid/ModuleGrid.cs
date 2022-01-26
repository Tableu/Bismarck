using UnityEngine;

public class ModuleGrid : MonoBehaviour
{
    public int ColumnLength;
    public int RowHeight;
    public GameObject[,] Grid;
    public GameObject Element;
    public RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[RowHeight, ColumnLength];
        for(int r = 0; r < RowHeight; r++)
        {
            for(int c = 0; c < ColumnLength; c++)
            {
                GameObject e = Instantiate(Element, transform, false);
                e.GetComponent<RectTransform>().anchoredPosition =new Vector3(c * 100, r * 100, 0);
                Grid[r, c] = e;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}