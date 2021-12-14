using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(fileName = "context", menuName = "Shared Resource/Starmap Context", order = 0)]
    public class Starmap : ScriptableObject
    {
        public StarmapGenerator generator;

        private void OnEnable()
        {
            StarSystems = generator.GenerateMap();
        }

        public List<StarSystem> StarSystems { get; private set; } = null;
        public StarSystem CurrentStarSystem { get; set; } = null;
    }
}