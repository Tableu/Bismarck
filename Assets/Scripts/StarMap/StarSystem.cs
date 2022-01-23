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
        public List<ShipData> Fleet;
        public List<Transform> FleetPositions;

        public void OnEnable()
        {
            Fleet = new List<ShipData>();
            FleetPositions = new List<Transform>();
        }

        public void OnDisable()
        {
            Fleet = null;
            FleetPositions = null;
        }

        /// <summary>
        /// Instances all objects needed to load the star system
        /// </summary>
        public void LoadSystem()
        {
            RandomFleet fleet = RandomFleetDB.fleetDB[Random.Range(0, RandomFleetDB.fleetDB.Count)];
            foreach (Transform child in fleet.fleetVisualsPrefab.transform)
            {
                FleetPositions.Add(child);
            }
            
            foreach (RandomShipList randomShip in fleet.randomFleet)
            {
                Fleet.Add(randomShip.RandomShip());
            }
        }
    }
}