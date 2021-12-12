using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util.RNG
{
    /// <summary>
    ///     Based on https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
    /// </summary>
    public class PoissonDisk
    {
        private readonly Texture2D _densityMap;
        private readonly int _k;
        private readonly int _numColumn;
        private readonly int _numRow;
        private readonly float _rMax;
        private readonly float _rMin;

        public PoissonDisk(int numRow, int numColumn, Texture2D densityMap, int k = 30)
        {
            _numRow = numRow;
            _numColumn = numColumn;
            _rMin = 1f / _numColumn;
            _rMax = 10*_rMin;
            _k = k;
            _densityMap = densityMap;
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

            var cellSize = _rMin / Mathf.Sqrt(2);
            // Step 1.
            var index = Random.Range(0, _numColumn * _numRow - 1);
            var activeList = new HashSet<int>();
            var points = new List<Vector2>();

            var x0 = index % _numRow;
            var y0 = index / _numColumn;
            activeList.Add(index);
            grid[x0, y0] = points.Count;
            points.Add(new Vector2(x0, y0) * cellSize);

            var texWidth = _densityMap.width;
            var texHeight = _densityMap.height;

            // Step 2.
            while (activeList.Count > 0)
            {
                var activeIndex = activeList.ElementAt(Random.Range(0, activeList.Count - 1));
                var activeX = activeIndex % _numRow;
                var activeY = activeIndex / _numColumn;
                var activeDensity = _densityMap.GetPixel(
                    Mathf.FloorToInt(Remap(activeX, 0, _numColumn, 0, texWidth)),
                    Mathf.FloorToInt(Remap(activeY, 0, _numRow, 0, texHeight))
                ).grayscale;

                var rActive = Remap(1 - activeDensity, 0, 1, _rMin, _rMax);

                int count, x = 0, y = 0;
                var point = new Vector2();
                var isValid = false;
                for (count = 0; count < _k && !isValid; count++)
                {
                    // Random point chosen uniformly from the disk
                    var radius = Random.Range(rActive, 2 * rActive);
                    var angle = Random.Range(0, 2 * Mathf.PI);
                    point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                    point += new Vector2(activeX, activeY) * cellSize;
                    // Check if it is within range of any near by points
                    x = (int) Mathf.Floor(point.x / cellSize);
                    y = (int) Mathf.Floor(point.y / cellSize);

                    var density = _densityMap.GetPixel(
                        Mathf.FloorToInt(Remap(x, 0, _numColumn, 0, texWidth)),
                        Mathf.FloorToInt(Remap(y, 0, _numRow, 0, texHeight))
                    ).grayscale;
                    isValid = x < _numColumn && x >= 0 && y < _numRow && y >= 0;

                    var r = Remap(1 - density, 0, 1, _rMin, _rMax);

                    var dist = Mathf.CeilToInt(r/cellSize);

                    for (var i = Math.Max(x - dist, 0); i <= Math.Min(x + dist, _numRow - 1) && isValid; i++)
                    for (var j = Math.Max(y - dist, 0); j <= Math.Min(y + dist, _numColumn - 1) && isValid; j++)
                    {
                        var idx = grid[i, j];
                        if (idx == -1) continue;
                        var otherPoint = points[idx];
                        var distance = (otherPoint - point).magnitude;
                        isValid = distance >= r;
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

            // Remove all points where texture is black
            var pointsToRemove = new HashSet<int>(points.Select((vector2, i) => new {vector2, i}).Where(i =>
            {
                var d = _densityMap.GetPixel(
                    Mathf.FloorToInt(Remap(i.vector2.x, 0, cellSize*_numColumn, 0, texWidth)),
                    Mathf.FloorToInt(Remap(i.vector2.y, 0, cellSize*_numColumn, 0, texHeight))
                ).grayscale;
                return d == 0;
            }).Select(i=>i.i));

            return points.Where((vector2, i) => !pointsToRemove.Contains(i)).ToList();
        }

        private static float Remap(float value, float fromLo, float fromHi, float toLo, float toHi)
        {
            var fromDelta = fromHi - fromLo;
            var toDelta = toHi - toLo;
            var normalizedInput = (value - fromLo) / fromDelta;
            return normalizedInput * toDelta + toLo;
        }
    }
}