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
    public void SpawnNodes(Vector3 startPos, Vector3 padding, int branchRate)
    {
        GenerateMap(branchRate);
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (_mazeGrid[row][col] == '.')
                {
                    var pos = new Vector3(startPos.x + col * padding.x, startPos.y + row * padding.y, 0);
                    GameObject.Instantiate(_nodePrefab, pos, Quaternion.identity, _nodeParent.transform);
                }
            }
        }
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
