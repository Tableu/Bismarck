using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    [SerializeField] private MapNode currentNode;
    [SerializeField] private PlayerIcon playerIcon;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject nodeParent;
    private List<GameObject> _mapNodes;
    private MapGenerator _mapGenerator;
    public static MapManager Instance
    {
        get { return _instance; }
    }

    public MapNode CurrentNode => currentNode;

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
        _mapGenerator = new MapGenerator(10, 10, nodePrefab, nodeParent,transform.position);
        currentNode = _mapGenerator.GenerateMap(transform.position, 4, 30);
        playerIcon.transform.position = currentNode.transform.position;
        DrawVisibleHyperLanes();
        //MovePlayer(currentNode);
    }
    
    void Update()
    {
        
    }

    public void MovePlayer(MapNode target)
    {
        playerIcon.transform.position = target.transform.position;
        currentNode = target;
        DrawVisibleHyperLanes();
    }

    public void AddMapNode(GameObject node)
    {
        _mapNodes.Add(node);
    }

    public void RemoveMapNode(GameObject node)
    {
        _mapNodes.Remove(node);
    }

    public void DrawMapNodes()
    {
        
    }

    public void DrawVisibleHyperLanes()
    {
        foreach(GameObject node in _mapNodes.ToList())
        {
            MapNode mapNode = node.GetComponent<MapNode>();
            if (mapNode != null)
            {
                mapNode.ResetMapNode();
            }
        }
        _mapNodes.Clear();
        _mapNodes.Add(currentNode.gameObject);
        currentNode.SetAdjacentNodes(8);
        currentNode.DrawHyperLanes();

        /*foreach(GameObject node in _mapNodes.ToList())
        {
            MapNode mapNode = node.GetComponent<MapNode>();
            if (mapNode != null)
            { 
                mapNode.SetAdjacentNodes(8);
                mapNode.DrawHyperLanes();
            }
        }*/
    }
}
