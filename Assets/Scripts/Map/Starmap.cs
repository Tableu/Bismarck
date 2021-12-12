using System.Collections.Generic;
using UnityEngine;
using Util.RNG;

namespace Map
{
    public class Starmap
    {
        public delegate void StarSystemUpdateHandler();

        public Starmap(int n, Texture2D texture2D)
        {
            // todo: generate fixed number of points
            var pointGenerator = new PoissonDisk(0.005f, 0.05f, texture2D);
            var systemCoordinates = pointGenerator.GeneratePoints();

            foreach (var coordinate in systemCoordinates)
                StarSystems.Add(new StarSystem
                {
                    Coordinates = coordinate*25
                });
        }

        public List<StarSystem> StarSystems { get; } = new List<StarSystem>();
        public event StarSystemUpdateHandler OnMapUpdate;
    }
}