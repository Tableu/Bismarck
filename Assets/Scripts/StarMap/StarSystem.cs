using System;
using System.Collections.Generic;
using UnityEngine;

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

        /// <summary>
        /// Instances all objects needed to load the star system
        /// </summary>
        public void LoadSystem()
        {
            // todo: call system loading code here (ie. fleet generation ect.)
        }
    }
}