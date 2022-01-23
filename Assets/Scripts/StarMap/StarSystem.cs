using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StarMap
{
    [Serializable]
    [CreateAssetMenu(fileName = "Star", menuName = "Map/StarSystem", order = 0)]
    public class StarSystem: ScriptableObject
    {
        public string SystemName;
        public Vector2 Coordinates;
        public StarType MainStar;
        public float StarSize;
        public FleetDBScriptableObject RandomFleetDB;

        /// <summary>
        /// Instances all objects needed to load the star system
        /// </summary>
        public void LoadSystem()
        {
            
        }
    }
}