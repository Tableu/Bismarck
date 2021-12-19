using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DelaunatorSharp;

namespace Utility
{
    public static class Triangulation
    {
        public static List<HashSet<int>> Delaunay(List<Vector2> points)
        {
            var mapping = points.Select(_ => new HashSet<int>()).ToList();
            var newPoints = points.Select(v => new Point(v.x, v.y)).Cast<IPoint>().ToArray();
            var triangulation = new Delaunator(newPoints);

            foreach (var halfEdge in triangulation.Halfedges)
            {
                var end = Delaunator.NextHalfedge(halfEdge);
                mapping[halfEdge].Add(end);
            }
            
            return mapping;
        }
    }
}