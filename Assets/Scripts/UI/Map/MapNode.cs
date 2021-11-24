using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public List<MapNode> adjacentNodes;
    [SerializeField] private Button nodeButton;
    [SerializeField] private GameObject hyperLanePrefab;
    private List<GameObject> hyperLanes;
    private bool _visited;
    private bool _linked;
    private bool _complete;
    private bool Linked => _linked;
    // Start is called before the first frame update
    private void Awake()
    {
        hyperLanes = new List<GameObject>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        MapManager.Instance.RemoveMapNode(gameObject);
    }

    public void VisitNode()
    {
        if (!_visited)
        {
            _visited = true;
            ColorBlock colorBlock = nodeButton.colors;
            colorBlock.normalColor = colorBlock.pressedColor;
            nodeButton.colors = colorBlock;
        }
    }

    public void DrawHyperLanes()
    {
        foreach (MapNode node in adjacentNodes)
        {
            if (!node._complete)
            {
                GameObject hyperLane = Instantiate(hyperLanePrefab, transform);
                LineRenderer hyperLaneRenderer = hyperLane.GetComponent<LineRenderer>();
                hyperLaneRenderer.SetPositions(new[]
                {
                    transform.position,
                    node.transform.position
                });
                if (!node._linked && !node._complete)
                {
                    MapManager.Instance.AddMapNode(node.gameObject);
                    node._linked = true;
                }

                hyperLanes.Add(hyperLane);
                node.hyperLanes.Add(hyperLane);
            }
        }
        _complete = true;
    }
}
