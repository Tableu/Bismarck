using System;
using Ships.Components;
using UnityEngine;

namespace Ships.DataManagment
{
    [Serializable]
    public struct ShipSaveData
    {
        public float healthPercentage;
        public Vector2 position;
        public string shipDataId;

        public ShipSaveData(GameObject ship)
        {
            healthPercentage = 0;
            var heath = ship.GetComponent<ShipHealth>();
            Debug.Assert(heath != null, "Missing health component on ship to save");
            if (heath != null)
            {
                healthPercentage = heath.PercentHealth;
            }

            position = ship.transform.position;
            shipDataId = ship.GetComponent<ShipStats>().Data.uuid;
        }
    }
}