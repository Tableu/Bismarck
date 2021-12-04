using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator
{
    private GameObject _nodePrefab;
    private GameObject _nodeParent;
    private MapNode[][] _mapNodeGrid;
    private int[][] _posGrid;
    private List<Vector2> _nodes;
    private int _columns;
    private int _rows;
    private Vector2 _startPos;

    public MapGenerator(int columns, int rows, GameObject nodePrefab, GameObject nodeParent, Vector2 startPos)
    {
        _columns = columns;
        _rows = rows;
        _nodePrefab = nodePrefab;
        _nodeParent = nodeParent;
        _startPos = startPos;
    }
    public MapNode GenerateMap(Vector2 startPos, float r, int branchRate)
    {
        _startPos = startPos;
        _mapNodeGrid = new MapNode[_rows][];
        for (int row = 0; row < _rows; row++)
        {
            _mapNodeGrid[row] = new MapNode[_columns];
            for (int column = 0; column < _columns; column++)
            {
                _mapNodeGrid[row][column] = null;
            }
        }
        PoissonDisk(r,20, branchRate);
        
        MapNode startNode = SpawnNodes();
        startNode.VisitNode();
        return startNode;
    }

    public MapNode SpawnNodes()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                int nodeIndex = _posGrid[row][column];
                if (nodeIndex >= 0)
                {
                    Vector2 nodePos = _nodes[nodeIndex];
                    GameObject node = GameObject.Instantiate(_nodePrefab, nodePos + _startPos, Quaternion.identity, parent:_nodeParent.transform);
                    if (node != null)
                    {
                        MapNode mapNode = node.GetComponent<MapNode>();
                        _mapNodeGrid[row][column] = mapNode;
                    }
                }
            }
        }
        
        MapNode startNode = null;
        while (startNode == null)
        {
            int randX = Random.Range(0, _columns);
            int randY = Random.Range(0, _rows);
            if (_mapNodeGrid[randY][randX] != null)
            {
                startNode = _mapNodeGrid[randY][randX];
            }
        }
        startNode.SetStartNode();
        return startNode;
    }

    private void PoissonDisk(float r, int k, int branchRate)
    {
        _posGrid = new int[_rows][];
        _nodes = new List<Vector2>();
        List<Vector2> activeList = new List<Vector2>();
        
        for (int row = 0; row < _rows; row++)
        {
            _posGrid[row] = new int[_columns];
            for (int col = 0; col < _columns; col++)
            {
                _posGrid[row][col] = -1;
            }
        }

        Vector2 initCoords = new Vector2(Random.Range(0, _columns - 1), Random.Range(0, _rows - 1));
        activeList.Add(initCoords);
        _nodes.Add(RandomCellPosition((int)initCoords.x, (int)initCoords.y,r));
        int posIndex = 0;
        _posGrid[(int) initCoords.y][(int) initCoords.x] = posIndex;
        
        while (activeList.Count > 0)
        {
            int i;
            float randomPos = Random.value;
            randomPos = Mathf.Pow(randomPos, Mathf.Exp(-branchRate));
            posIndex = ((int) randomPos * activeList.Count) - 1;
            int col = (int)activeList[posIndex].x;
            int row = (int)activeList[posIndex].y;
            for (i = 0; i < k; i++)
            {
                float randX = Random.Range(r, r*2);
                randX = Random.Range(0f, 1f) < 0.5 ? -randX : randX;
                float randY = Random.Range(r, r*2);
                randY = Random.Range(0f, 1f) < 0.5 ? -randY : randY;
                Vector2? result = CheckPoissonPoint(new Vector2(randX, randY), col, row, r);
                if (result != null)
                {
                    activeList.Add(result.Value);
                    break;
                }
            }

            if (i >= k)
            {
                activeList.RemoveAt(posIndex);
            }
        }
    }

    private Vector2? CheckPoissonPoint(Vector2 posOffset, int col, int row, float r)
    {
        int nodeIndex = _posGrid[row][col];
        Vector2 centerPos = _nodes[nodeIndex]+posOffset;
        col = Mathf.FloorToInt(centerPos.x / (r / Mathf.Sqrt(2)));
        row = Mathf.FloorToInt(centerPos.y / (r / Mathf.Sqrt(2)));
        if (col < 0 || col > _columns - 1 || row < 0 || row > _rows - 1 || _posGrid[row][col] != -1)
        {
            return null;
        }
        if (col > 0)
        {
            if (row > 0)
            {
                nodeIndex = _posGrid[row-1][col-1];
                if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                    return null;
            }
            if (row < _rows - 1)
            {
                nodeIndex = _posGrid[row+1][col-1];
                if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                    return null;
            }
            nodeIndex = _posGrid[row][col-1];
            if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                return null;
        }

        if (col < _columns - 1)
        {
            if (row > 0)
            {
                nodeIndex = _posGrid[row-1][col+1];
                if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                    return null;
            }
            if (row < _rows - 1)
            {
                nodeIndex = _posGrid[row+1][col+1];
                if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                    return null;
            }
            nodeIndex = _posGrid[row][col+1];
            if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                return null;
        }

        if (row > 0)
        {
            nodeIndex = _posGrid[row-1][col];
            if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                return null;
        }

        if (row < _rows - 1)
        {
            nodeIndex = _posGrid[row+1][col];
            if (nodeIndex != -1 && (centerPos-_nodes[nodeIndex]).magnitude < r)
                return null;
        }
        _nodes.Add(centerPos);
        _posGrid[row][col] = _nodes.Count - 1;
        
        return new Vector2(col, row);
    }
    
    private List<MapNode> GetAdjacentMapNodes(int row, int col)
    {
        List<MapNode> nodeList = new List<MapNode>();
        if (row > 0)
        {
            MapNode node = _mapNodeGrid[row - 1][col];
            if (node != null)
            {
                nodeList.Add(node);
            }
            if (col > 0)
            {
                node = _mapNodeGrid[row - 1][col-1];
                if (node != null)
                {
                    nodeList.Add(node);
                }
            }

            if (col < _columns - 1)
            {
                node = _mapNodeGrid[row - 1][col+1];
                if (node != null)
                {
                    nodeList.Add(node);
                }
            }
        }

        if (row < _rows - 1)
        {
            MapNode node = _mapNodeGrid[row + 1][col];
            if (node != null)
            {
                nodeList.Add(node);
            }
            if (col > 0)
            {
                node = _mapNodeGrid[row + 1][col-1];
                if (node != null)
                {
                    nodeList.Add(node);
                }
            }

            if (col < _columns - 1)
            {
                node = _mapNodeGrid[row + 1][col+1];
                if (node != null)
                {
                    nodeList.Add(node);
                }
            }
            
        }

        if (col > 0)
        {
            MapNode node = _mapNodeGrid[row][col - 1];
            if (node != null)
            {
                nodeList.Add(node);
            }
            
        }

        if (col < _columns - 1)
        {
            MapNode node = _mapNodeGrid[row][col + 1];
            if (node != null)
            {
                nodeList.Add(node);
            }
        }

        return nodeList;
    }

    private Vector2 RandomCellPosition(int x, int y, float r)
    {
        float randX = Random.Range(0,r/Mathf.Sqrt(2));
        randX = Random.Range(0f, 1f) < 0.5 ? -randX : randX;
        float randY = Random.Range(0, r/Mathf.Sqrt(2));
        randY = Random.Range(0f, 1f) < 0.5 ? -randY : randY;
        
        return new Vector2(x*(r/Mathf.Sqrt(2))+randX,y*(r/Mathf.Sqrt(2))+randY);
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count-1; i++)
        {
            T tmp = list[i];
            int r = Random.Range(i, list.Count);
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
