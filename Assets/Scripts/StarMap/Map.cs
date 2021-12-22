using System.Collections.Generic;
using UnityEngine;

namespace StarMap
{
    [CreateAssetMenu(fileName = "Map", menuName = "StarMap", order = 0)]
    public class Map : ScriptableObject
    {
        public List<StarSystem> StarSystems;
    }
}