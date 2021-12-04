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
                    colorBlock.normalColor = colorBlock.highlightedColor;
                    nodeButton.colors = colorBlock;
                    MapManager.Instance.MovePlayer(this);
                    return;
                } 
            }
        }
    }

    public void ResetMapNode()
    {
        adjacentNodes = new List<MapNode>();
        _complete = false;
        foreach (HyperLane hyperLane in hyperLanes)
        {
            Destroy(hyperLane.hyperLane);
        }
        hyperLanes = new List<HyperLane>();
    }

    public void DrawHyperLanes()
    {
        foreach (MapNode node in adjacentNodes)
        {
            if (node._complete)
                continue;
            GameObject hyperLane = Instantiate(hyperLanePrefab, transform);
            LineRenderer hyperLaneRenderer = hyperLane.GetComponent<LineRenderer>();
            hyperLaneRenderer.SetPositions(new[]
            {
                transform.position, 
                node.transform.position
            });
            MapManager.Instance.AddMapNode(node.gameObject);

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
        _complete = true;
    }

    public void SetStartNode()
    {
        ColorBlock colorBlock = nodeButton.colors;
        colorBlock.normalColor = colorBlock.highlightedColor;
        nodeButton.colors = colorBlock;
    }

    public void SetAdjacentNodes(float fuel)
    {
        adjacentNodes = new List<MapNode>();
        if (fuel <= 0)
            return;
        
        CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = fuel;
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.layerMask = LayerMask.GetMask("UI");
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        circleCollider.Cast(new Vector2(0, 0), contactFilter2D, results, ignoreSiblingColliders:true);
        foreach (RaycastHit2D raycastHit2D in results)
        {
            var mapNode = raycastHit2D.transform.gameObject.GetComponent<MapNode>();
            if (mapNode != null)
            {
                adjacentNodes.Add(mapNode);
            }
        }
        Destroy(circleCollider);
    }
}
