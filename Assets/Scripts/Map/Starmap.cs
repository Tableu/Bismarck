using System.Collections.Generic;
using UnityEngine;
using Util.RNG;

namespace Map
{
    public class Starmap
    {
        private int _seed;

        public Starmap(int numSystems, int seed)
        {
            _seed = seed;
            Random.InitState(seed);
            // todo: generate fixed number of points
            var pointGenerator = new PoissonDisk(10, 10);
            var systemCoordinates = pointGenerator.GeneratePoints();

            foreach (var coordinate in systemCoordinates)
                StarSystems.Add(new StarSystem
                {
                    Coordinates = coordinate
                });
        }

        public List<StarSystem> StarSystems { get; } = new List<StarSystem>();

        public delegate void StarSystemUpdateHandler();
        public event StarSystemUpdateHandler OnMapUpdate;
    }
}