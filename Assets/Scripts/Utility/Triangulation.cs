using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using UnityEngine;

namespace Utility
{
    public static class Triangulation
    {
        public static List<Vertex> Delaunay(List<Vector2> points)
        {
            var mapping = points.Select(p => new Vertex {Point = p, Connections = new List<int>()}).ToList();
            var newPoints = points.Select(v => new Point(v.x, v.y)).Cast<IPoint>().ToArray();
            var triangulation = new Delaunator(newPoints);

            for (var e = 0; e < triangulation.Triangles.Length; e++)
            {
                if (e <= triangulation.Halfedges[e]) continue;
                var start = triangulation.Triangles[e];
                var end = triangulation.Triangles[Delaunator.NextHalfedge(e)];
                mapping[start].Connections.Add(end);
                mapping[end].Connections.Add(start);
            }

            return mapping;
        }

        public struct Vertex
        {
            public Vector2 Point;
            public List<int> Connections;
        }
    }
}