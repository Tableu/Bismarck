using System;
using System.Collections.Generic;
using System.Linq;
using Editor;
using Ships.Components;
using Ships.DataManagement;
using UnityEngine;

namespace Ships.Fleets
{
    /// <summary>
    ///     Manages a group of ships.
    /// </summary>
    public class FleetManager : MonoBehaviour
    {

        private static readonly HashSet<FleetManager> ActiveFleets = new HashSet<FleetManager>();

        [Header("Fleet Config")]
        [SerializeField] private string fleetName;
        [ReadOnlyField] [SerializeField] private int fleetId;
        [SerializeField] private GameObject shipBasePrefab;

        private readonly Dictionary<FleetManager, FleetAgroStatus> _agroStatusMap = new Dictionary<FleetManager, FleetAgroStatus>();

        public string FleetName => fleetName;
        public int FleetId => fleetId;
        public IReadOnlyDictionary<FleetManager, FleetAgroStatus> AgroStatusMap => _agroStatusMap;


        private void Awake()
        {
            fleetId = fleetName.GetHashCode();
            _agroStatusMap[this] = FleetAgroStatus.Self;
            foreach (var fleet in ActiveFleets)
            {
                _agroStatusMap[fleet] = FleetAgroStatus.Neutral;
            }
        }
        private void OnEnable()
        {
            Debug.Assert(ActiveFleets.Count(o => o.fleetId == fleetId) == 0, $"Multiple fleets created with matching ids {fleetName}[{fleetId}]");
            ActiveFleets.Add(this);
            OnActiveFleetsChanged?.Invoke(null, this);
            OnActiveFleetsChanged += HandleActiveFleetChange;
        }
        private void OnDisable()
        {
            ActiveFleets.Remove(this);
            OnActiveFleetsChanged -= HandleActiveFleetChange;
            OnActiveFleetsChanged?.Invoke(this, null);
        }
        private static event Action<FleetManager, FleetManager> OnActiveFleetsChanged;

        /// <summary>
        ///     Get the fleet a ship belongs to.
        /// </summary>
        /// <param name="ship">The ships root gameobject</param>
        /// <returns>The ships fleet, null if no fleet was found</returns>
        /// <returns>null if no fleet was found</returns>
        public static FleetManager GetFleet(GameObject ship)
        {
            var info = ship.GetComponent<ShipInfo>();
            return info == null ? null : info.Fleet;
        }

        /// <summary>
        ///     Spawns a ship from ShipData and adds it to the fleet
        /// </summary>
        /// <param name="shipData">The ship data to create a ship from. Must not be null.</param>
        /// <param name="position">The position to spawn the ship, relative to the fleet transform</param>
        /// <returns>The new ships root object</returns>
        public GameObject SpawnShip(ShipData shipData, Vector2 position)
        {
            var newShip = Instantiate(shipBasePrefab, position, Quaternion.identity, transform);
            var info = newShip.GetComponent<ShipInfo>();
            info.Initialize(shipData);
            // todo: move ships to avoid collisions
            return newShip;
        }

        /// <summary>
        ///     Changes this fleets aggressiveness towards another fleet
        /// </summary>
        /// <param name="targetFleet">The fleet to change aggressiveness towards. Must not be its self.</param>
        /// <param name="newStatus">The new aggressiveness status</param>
        public void ChangeAgroStatus(FleetManager targetFleet, FleetAgroStatus newStatus)
        {
            Debug.Assert(targetFleet != this, "Attempting to change fleet agro status to self");
            _agroStatusMap[targetFleet] = newStatus;
        }
        private void HandleActiveFleetChange(FleetManager prev, FleetManager next)
        {
            if (prev == null)
            {
                _agroStatusMap[next] = FleetAgroStatus.Neutral;
            }
            else
            {
                _agroStatusMap.Remove(prev);
            }
        }
        // Editor only debug variables/functions
        // todo: see if this can be made into a custom inspector instead
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private ShipData testShip;
        [SerializeField] private Vector2 testSpawnPoint;

        [ContextMenu("Spawn Ship")]
        private void TestSpawn()
        {
            SpawnShip(testShip, testSpawnPoint);
        }
#endif
    }

    public enum FleetAgroStatus
    {
        Self,
        Allied,
        Neutral,
        Hostile
    }

}
