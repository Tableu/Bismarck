using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct HyperLane
{
    public MapNode mapNode;
    public GameObject hyperLane;
}
public class MapNode : MonoBehaviour
{
    public List<MapNode> adjacentNodes;
    [SerializeField] private Button nodeButton;
    [SerializeField] private GameObject hyperLanePrefab;
    private List<HyperLane> hyperLanes;
    private bool _visited;
    private bool _linked; //node has been added
    private bool _complete; //All hyperlanes drawn
    private bool Linked => _linked;
    // Start is called before the first frame update
    private void Awake()
    {
        hyperLanes = new List<HyperLane>();
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
        MapNode currentNode = MapManager.Instance.CurrentNode;
        foreach (HyperLane lane in hyperLanes.ToList())
        {
            LineRenderer laneRenderer = lane.hyperLane.GetComponent<LineRenderer>();
            if (laneRenderer != null)
            {
                GameObject node = lane.mapNode.gameObject;
                if (node != null && node.Equals(currentNode.gameObject))
                {
                    _visited = true;
                    laneRenderer.startColor = Color.green;
                    laneRenderer.endColor = Color.green;
                        
                    ColorBlock colorBlock = nodeButton.colors;
                    colorBlock.normalColor = colorBlock.pressedColor;
                    colorBlock.selectedColor = colorBlock.pressedColor;
                    nodeButton.colors = colorBlock;
                    MapManager.Instance.MovePlayer(this);
                } 
            }
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
                
                hyperLanes.Add(new HyperLane
                {
                    hyperLane = hyperLane,
                    mapNode = node
                });

                node.hyperLanes.Add(new HyperLane
                {
                    hyperLane = hyperLane,
                    mapNode = this
                });
            }
        }
        _complete = true;
    }
}
