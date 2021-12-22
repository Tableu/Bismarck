using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Utility;

namespace Tests
{
    public class GeometryTests
    {
        [Test]
        public void GetConvexHull_InvalidPointInput()
        {
            var inputPoints = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
            };

            var hull = Geometry.GetConvexHull(inputPoints);
            Assert.IsNull(hull);
        }

        [Test]
        public void GetConvexHull_ThreePointInput()
        {
            var inputPoints = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };

            var expectedHull = new List<int>
            {
                0, 1, 2
            };
            
            var hull = Geometry.GetConvexHull(inputPoints);
            Assert.AreEqual(expectedHull, hull);
        }

        [Test]
        public void GetConvexHull_QuadInput()
        {
            var inputPoints = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
            };

            var expectedHull = new List<int>
            {
                2, 3, 0, 1
            };
            
            var hull = Geometry.GetConvexHull(inputPoints);
            Assert.AreEqual(expectedHull, hull);
        }
        
        [Test]
        public void GetConvexHull_QuadInputWithInteriorPoints()
        {
            var inputPoints = new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(0, 0),
                new Vector2(0.1f, 0.25f),
                new Vector2(0.1f, 0.2f),
                new Vector2(0.75f, 0.2f),
                new Vector2(0.75f, 0.72f),
            };

            var expectedHull = new List<int>
            {
                2, 3, 0, 1
            };
            
            var hull = Geometry.GetConvexHull(inputPoints);
            Assert.AreEqual(expectedHull, hull);
        }
    }
}