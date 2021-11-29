using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Node
{
    public List<Node> adjacentNodes;
    public Vector3 position;
}

public class MapGenerator
{
    private GameObject _nodePrefab;
    private GameObject _nodeParent;
    private char[][] _mazeGrid;
    private MapNode[][] _mapNodeGrid;
    private int[][] _posGrid;
    private List<Vector2> _nodes;
    private List<Vector2> _frontier;
    private int _columns;
    private int _rows;

    public MapGenerator(int columns, int rows, GameObject nodePrefab, GameObject nodeParent)
    {
        _columns = columns;
        _rows = rows;
        _nodePrefab = nodePrefab;
        _nodeParent = nodeParent;
    }
    public MapNode SpawnNodes(Vector3 startPos, float r, int branchRate)
    {
        GenerateMap(branchRate);
        PoissonDisk(r,20);
        _mapNodeGrid = new MapNode[_rows][];
        for (int row = 0; row < _rows; row++)
        {
            _mapNodeGrid[row] = new MapNode[_columns];
            for (int col = 0; col < _columns; col++)
            {
                if (_mazeGrid[row][col] == '.')
                {
                    int posIndex = _posGrid[row][col];
                    if (posIndex != -1)
                    {
                        GameObject node = GameObject.Instantiate(_nodePrefab, _nodes[posIndex] + (Vector2)startPos, Quaternion.identity,
                            _nodeParent.transform);

                        if (node != null)
                        {
                            MapNode mapNode = node.GetComponent<MapNode>();
                            _mapNodeGrid[row][col] = mapNode;
                        }
                    }
                }
            }
        }
       
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (_mazeGrid[row][col] == '.')
                {
                    MapNode mapNode = _mapNodeGrid[row][col];
                    if (mapNode != null)
                    {
                        List<MapNode> adjacentNodes = GetAdjacentNodes(mapNode,row, col);
                        mapNode.adjacentNodes = adjacentNodes;
                    }
                }
            }
        }

        MapNode startNode = null;
        while (startNode == null)
        {
            int randX = Random.Range(0, _columns);
            int randY = Random.Range(0, _rows);
            if (_mazeGrid[randY][randX] == '.')
            {
                startNode = _mapNodeGrid[randY][randX];
            }
        }
        return startNode;
    }

    private void PoissonDisk(float r, int k)
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

        float randX = Random.Range(0,r/Mathf.Sqrt(2));
        randX = Random.Range(0f, 1f) < 0.5 ? -randX : randX;
        float randY = Random.Range(0, r/Mathf.Sqrt(2));
        randY = Random.Range(0f, 1f) < 0.5 ? -randY : randY;
        
        Vector2 initCoords = new Vector2(Random.Range(0, _columns - 1), Random.Range(0, _rows - 1));
        activeList.Add(initCoords);
        _nodes.Add(new Vector2(initCoords.x*(r/Mathf.Sqrt(2))+randX,initCoords.y*(r/Mathf.Sqrt(2))+randY));
        int posIndex = 0;
        _posGrid[(int) initCoords.y][(int) initCoords.x] = posIndex;
        
        while (activeList.Count > 0)
        {
            int i;
            posIndex = Random.Range(0, activeList.Count - 1);
            int col = (int)activeList[posIndex].x;
            int row = (int)activeList[posIndex].y;
            for (i = 0; i < k; i++)
            {
                randX = Random.Range(r, 2 * r);
                randX = Random.Range(0f, 1f) < 0.5 ? -randX : randX;
                randY = Random.Range(r, 2 * r);
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
    
    //http://pcg.wdfiles.com/local--files/pcg-algorithm%3Amaze/growingtree.py by d.factorial@gmail.com
    //'#' is empty space
    //'.' is a mapNode
    //',' is exposed but undetermined
    //'?' is unexposed and undetermined
    private void GenerateMap(int branchRate)
    {
        _mazeGrid = new char[_rows][];
        _frontier = new List<Vector2>();
        for (int row = 0; row < _rows; row++)
        {
            _mazeGrid[row] = new char[_columns];
            for (int col = 0; col < _columns; col++)
            {
                _mazeGrid[row][col] = '?';
            }
        }

        int colChoice = Random.Range(0, _columns - 1);
        int rowChoice = Random.Range(0, _rows - 1);
        Carve(rowChoice,colChoice);

        float pos;
        Vector2 choice;
        while (_frontier.Count > 0)
        {
            pos = Random.value;
            pos = Mathf.Pow(pos, Mathf.Exp(-branchRate));
            choice = _frontier[((int) pos * _frontier.Count)-1];
            if (Check((int) choice.y, (int) choice.x))
            {
                Carve((int)choice.y,(int)choice.x);
            }
            else
            {
                Harden((int)choice.y,(int)choice.x);
            }

            _frontier.Remove(choice);
        }
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (_mazeGrid[row][col] == '?')
                {
                    _mazeGrid[row][col] = '#';
                }
            }
        }
    }

    private void Carve(int row, int column)
    {
        _mazeGrid[row][column] = '.';
        List<Vector2> extra = new List<Vector2>();
        if (column > 0)
        {
            if (_mazeGrid[row][column - 1] == '?')
            {
                _mazeGrid[row][column - 1] = ',';
                extra.Add(new Vector2(column-1,row));
            }
        }
        if (column < _columns - 1)
        {
            if (_mazeGrid[row][column + 1] == '?')
            {
                _mazeGrid[row][column + 1] = ',';
                extra.Add(new Vector2(column+1,row));
            }
        }
        if (row > 0)
        {
            if (_mazeGrid[row-1][column] == '?')
            {
                _mazeGrid[row-1][column] = ',';
                extra.Add(new Vector2(column,row-1));
            }
        }
        if (row < _rows - 1)
        {
            if (_mazeGrid[row+1][column] == '?')
            {
                _mazeGrid[row+1][column] = ',';
                extra.Add(new Vector2(column,row+1));
            }
        }
        
        ShuffleList(extra);
        _frontier.AddRange(extra);
    }

    private void Harden(int row, int column)
    {
        _mazeGrid[row][column] = '#';
    }

    private bool Check(int row, int column)
    {
        int edgestate = 0;
        if (column > 0)
        {
            if (_mazeGrid[row][column - 1] == '.')
                edgestate += 1;
        }
        if (column < _columns - 1)
        {
            if (_mazeGrid[row][column + 1] == '.')
                edgestate += 2;
        }
        if (row > 0)
        {
            if (_mazeGrid[row - 1][column] == '.')
                edgestate += 4;
        }
        if (row < _rows - 1)
        {
            if (_mazeGrid[row + 1][column] == '.')
                edgestate += 8;
        }

        switch (edgestate)
        {
            case 1:
                if (column < _columns - 1)
                {
                    if (row > 0)
                    {
                        if (_mazeGrid[row - 1][column + 1] == '.')
                            return false;
                    }

                    if (row < _rows - 1)
                    {
                        if (_mazeGrid[row + 1][column + 1] == '.')
                            return false;
                    }
                }
                break;
            case 2:
                if (column > 0)
                {
                    if (row > 0)
                    {
                        if (_mazeGrid[row - 1][column - 1] == '.')
                            return false;
                    }

                    if (row < _rows - 1)
                    {
                        if (_mazeGrid[row + 1][column - 1] == '.')
                            return false;
                    }
                }
                break;
            case 4:
                if (row < _rows - 1)
                {
                    if (column > 0)
                    {
                        if (_mazeGrid[row + 1][column - 1] == '.')
                            return false;
                    }

                    if (column < _columns - 1)
                    {
                        if (_mazeGrid[row + 1][column + 1] == '.')
                            return false;
                    }
                }
                break;
            case 8:
                if (row > 0)
                {
                    if (column > 0)
                    {
                        if (_mazeGrid[row - 1][column - 1] == '.')
                            return false;
                    }

                    if (column < _columns - 1)
                    {
                        if (_mazeGrid[row - 1][column + 1] == '.')
                            return false;
                    }
                }
                break;
        }

        int[] states = {1, 2, 4, 8};
        foreach (int state in states)
        {
            if (edgestate == state)
                return true;
        }
        return false;
    }

    private List<MapNode> GetAdjacentNodes(MapNode mapNode,int row, int col)
    {
        List<MapNode> nodeList = new List<MapNode>();
        if (row > 0)
        {
            if (_mazeGrid[row - 1][col] == '.')
            {
                MapNode node = _mapNodeGrid[row - 1][col];
                if (node != null)
                {
                    nodeList.Add(node);
                    node.adjacentNodes.Add(mapNode);
                }
            }
        }

        if (row < _rows - 1)
        {
            if (_mazeGrid[row + 1][col] == '.')
            {
                MapNode node = _mapNodeGrid[row + 1][col];
                if (node != null)
                {
                    nodeList.Add(node);
                    node.adjacentNodes.Add(mapNode);
                }
            }
        }

        if (col > 0)
        {
            if (_mazeGrid[row][col - 1] == '.')
            {
                MapNode node = _mapNodeGrid[row][col - 1];
                if (node != null)
                {
                    nodeList.Add(node);
                    node.adjacentNodes.Add(mapNode);
                }
            }
        }

        if (col < _columns - 1)
        {
            if (_mazeGrid[row][col + 1] == '.')
            {
                MapNode node = _mapNodeGrid[row][col + 1];
                if (node != null)
                {
                    nodeList.Add(node);
                    node.adjacentNodes.Add(mapNode);
                }
            }
        }

        return nodeList;
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
