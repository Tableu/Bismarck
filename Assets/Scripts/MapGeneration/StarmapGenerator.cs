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
            var pointGenerator = new PoissonDisk(0.005f, 0.05f, _densityMap);
            var systemCoordinates = pointGenerator.GeneratePoints();

            var starSystems = systemCoordinates.Select(coordinate =>
                new StarSystem {Coordinates = coordinate * 25}).ToList();
            return starSystems;
        }
    }
}