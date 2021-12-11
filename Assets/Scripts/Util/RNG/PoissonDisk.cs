using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util.RNG
{
    /// <summary>
    /// Based on https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
    /// </summary>
    public class PoissonDisk
    {
        private readonly int _k;
        private readonly float _minimumDistance;
        private readonly int _numColumn;

        private readonly int _numRow;

        public PoissonDisk(int numRow, int numColumn, float minimumDistance = 1, int k = 30)
        {
            _numRow = numRow;
            _numColumn = numColumn;
            _k = k;
            _minimumDistance = minimumDistance;
        }

        public List<Vector2> GeneratePoints(int seed)
        {
            Random.InitState(seed);
            return GeneratePoints();
        }

        public List<Vector2> GeneratePoints()
        {
            // Step 0.
            var grid = new int[_numRow, _numColumn];
            for (var x = 0; x < grid.GetLength(0); x++)
            for (var y = 0; y < grid.GetLength(1); y++)
                grid[x, y] = -1;

            var cellSize = _minimumDistance / Mathf.Sqrt(2);
            // Step 1.
            var index = Random.Range(0, _numColumn * _numRow - 1);
            var activeList = new HashSet<int>();
            var points = new List<Vector2>();

            var x0 = index % _numRow;
            var y0 = index / _numColumn;
            activeList.Add(index);
            grid[x0, y0] = points.Count;
            points.Add(new Vector2(x0, y0) * cellSize);

            // Step 2.
            while (activeList.Count > 0)
            {
                var activeIndex = activeList.ElementAt(Random.Range(0, activeList.Count - 1));
                var activeX = activeIndex % _numRow;
                var activeY = activeIndex / _numColumn;

                int count, x = 0, y = 0;
                var point = new Vector2();
                var isValid = false;
                for (count = 0; count < _k && !isValid; count++)
                {
                    // Random point chosen uniformly from the disk
                    var radius = Random.Range(_minimumDistance, 2 * _minimumDistance);
                    var angle = Random.Range(0, 2 * Mathf.PI);
                    point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                    point += new Vector2(activeX, activeY) * cellSize;
                    // Check if it is within range of any near by points
                    x = (int) Mathf.Floor(point.x / cellSize);
                    y = (int) Mathf.Floor(point.y / cellSize);
                    isValid = x < _numColumn && x >= 0 && y < _numRow && y >= 0;

                    for (var i = Math.Max(x - 2, 0); i <= Math.Min(x + 2, _numRow - 1) && isValid; i++)
                    for (var j = Math.Max(y - 2, 0); j <= Math.Min(y + 2, _numColumn - 1) && isValid; j++)
                    {
                        var idx = grid[i, j];
                        if (idx == -1) continue;
                        var otherPoint = points[idx];
                        var distance = (otherPoint - point).magnitude;
                        isValid = distance >= _minimumDistance;
                    }
                }

                if (isValid)
                {
                    grid[x, y] = points.Count;
                    points.Add(point);
                    activeList.Add(x + y * _numColumn);
                    continue;
                }

                activeList.Remove(activeIndex);
            }

            return points;
        }
    }
}