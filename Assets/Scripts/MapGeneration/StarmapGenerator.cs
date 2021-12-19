using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace MapGeneration
{
    [CreateAssetMenu(fileName = "mapGen", menuName = "Database/Starmap Generator", order = 0)]
    public class StarmapGenerator : ScriptableObject
    {
        [Header("Generation Parameters")] [SerializeField]
        private Texture2D _densityMap;

        [SerializeField] private List<StarType> _starTypes;

        public List<StarSystem> GenerateMap()
        {
            var pointGenerator = new PoissonDisk(0.01f, 0.1f, _densityMap);
            var systemCoordinates = pointGenerator.GeneratePoints();
            var connectedSystems = Triangulation.Delaunay(systemCoordinates);

            var starSystems = connectedSystems.Select(vertex =>
                new StarSystem {Coordinates = (vertex.Point - 0.5f*Vector2.one) * 25, ConnectedSystems = vertex.Connections}).ToList();
            return starSystems;
        }
    }
}