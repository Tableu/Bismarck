using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    [SerializeField] private MapNode startNode;
    private List<GameObject> _mapNodes;
    private int _index;
    public static MapManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            _instance = null;
            return;
        }
        _instance = this;

        _mapNodes = new List<GameObject>();
    }

    void Start()
    {
        _mapNodes.Add(startNode.gameObject);
        _index = _mapNodes.Count;
        
        startNode.DrawHyperLanes();
        DrawVisibleHyperLanes(1);
    }
    
    void Update()
    {
        
    }

    public void AddMapNode(GameObject node)
    {
        _mapNodes.Add(node);
    }

    public void RemoveMapNode(GameObject node)
    {
        _mapNodes.Remove(node);
    }

    public void DrawVisibleHyperLanes(int maxDepth)
    {
        int endIndex = _mapNodes.Count;
        
        for(int depth = 0; depth < maxDepth; depth++)
        {
            while(_index < endIndex)
            {
                MapNode mapNode = _mapNodes[_index].GetComponent<MapNode>();
                if (mapNode != null)
                {
                    mapNode.DrawHyperLanes();
                }
                _index++;
            }
            endIndex = _mapNodes.Count;
        }
    }
}
