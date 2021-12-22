using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarMap
{
    [Serializable]
    public class StarSystem
    {
        public string SystemName;
        public Vector2 Coordinates;
        public Star MainStar;
        public List<StarLane> StarLanes = new List<StarLane>();
    }
}