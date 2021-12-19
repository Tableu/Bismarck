using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class StarSystem
    {
        public Vector2 Coordinates;
        public StellarObject MainStar;
        public List<int> ConnectedSystems;
        public HashSet<int> HyperLanes;
    }
}