using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Utility
{
    public static class Geometry
    {
        public enum Orientation
        {
            RightTurn = -1,
            Indeterminate = 0,
            LeftTurn = 1
        }

        public static int InCircle(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            var mat = new Matrix4x4(
                new Vector4(a.x, a.y, a.sqrMagnitude, 1),
                new Vector4(b.x, b.y, b.sqrMagnitude, 1),
                new Vector4(c.x, c.y, c.sqrMagnitude, 1),
                new Vector4(d.x, d.y, d.sqrMagnitude, 1)
            );
            return (int) Mathf.Sign(mat.determinant);
        }

        public static List<int> GetConvexHull(List<Vector2> points)
        {
            // https://en.wikipedia.org/wiki/Gift_wrapping_algorithm


            if (points.Count < 3) return null;
            var convexHullPoints = new List<int>();
            var inputPoints = new HashSet<int>(points.Select((v, i) => i).ToArray());

            // get initial point
            var currentPointIdx = 0;
            var comp = new LexicographicalVectorCompare();
            for (var i = 0; i < points.Count; i++)
                if (comp.Compare(points[i], points[currentPointIdx]) < 0)
                    currentPointIdx = i;

            while (true)
            {
                convexHullPoints.Add(currentPointIdx);
                var nextPoint = inputPoints.First();
                var currentPoint = points[currentPointIdx];

                foreach (var i in inputPoints)
                    if (SideOfOrientedLine(currentPoint, points[nextPoint], points[i]) == Orientation.LeftTurn ||
                        currentPointIdx == nextPoint)
                        nextPoint = i;
                currentPointIdx = nextPoint;
                if (currentPointIdx == convexHullPoints[0]) break;
                inputPoints.Remove(currentPointIdx);
            }

            return convexHullPoints;
        }

        public static Orientation SideOfOrientedLine(Vector2 a, Vector2 b, Vector2 c)
        {
            var mat = new float3x3(
                new float3(a.x, b.x, c.x),
                new float3(a.y, b.y, c.y),
                new float3(1, 1, 1)
            );
            var det = math.determinant(mat);
            if (Mathf.Approximately(det, 0))
            {
                return Orientation.Indeterminate;
            }
            return (Orientation) (int) math.sign(det);
        }

        public static bool IsStrictlyConvexQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            return false;
        }

        private class LexicographicalVectorCompare : IComparer<Vector2>
        {
            public int Compare(Vector2 a, Vector2 b)
            {
                var result = 1;
                if (a.x < b.x) result = -1;
                else if (Mathf.Approximately(a.x, b.x) && a.y < b.y) result = -1;
                return result;
            }
        }
    }
}