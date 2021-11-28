using System;
using System.Collections.Generic;
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
        _mapGenerator = new MapGenerator(30, 30, nodePrefab, nodeParent);
    }

    void Start()
    {
        DrawVisibleHyperLanes(1);
        _mapGenerator.SpawnNodes(new Vector3(0, 0f,0f), new Vector3(3,3,0), 30);
        //MovePlayer(currentNode);
    }
    
    void Update()
    {
        
    }

    public void MovePlayer(MapNode target)
    {
        playerIcon.transform.position = target.transform.position;
        currentNode = target;
        DrawVisibleHyperLanes(1);
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

    public void DrawVisibleHyperLanes(int maxDepth)
    {
        _mapNodes.Clear();
        _mapNodes.Add(currentNode.gameObject);
        currentNode.DrawHyperLanes();
        int endIndex = _mapNodes.Count;
        int index = 0;
        for(int depth = 0; depth < maxDepth; depth++)
        {
            while(index < endIndex)
            {
                MapNode mapNode = _mapNodes[index].GetComponent<MapNode>();
                if (mapNode != null)
                {
                    mapNode.DrawHyperLanes();
                } 
                index++;
            }
            endIndex = _mapNodes.Count;
        }
    }
}
