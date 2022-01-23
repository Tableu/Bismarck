using System;
using Ships.Components;
using UnityEngine;

namespace Ships.DataManagement
{
    /// <summary>
    /// A struct that defines the save format/data for ships.
    /// </summary>
    [Serializable]
    public struct ShipSaveData
    {
        /// <summary>
        /// The ships current health
        /// </summary>
        public float healthPercentage;

        /// <summary>
        /// The ships current postion
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// The UUID for the ship data scriptable object
        /// </summary>
        public string shipDataId;

        /// <summary>
        /// Constructs the save data from a ship scriptable object
        /// </summary>
        /// <param name="ship">The ship game object</param>
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