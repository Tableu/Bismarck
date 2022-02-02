using Ships.Fleets;
using StarMap;
using UnityEngine;

public class LoadEnemies : MonoBehaviour
{
    // [SerializeField] private ShipSpawner enemyShipSpawner;
    [SerializeField] private MapContext mapContext;
    [SerializeField] private Transform fleetParent;
    [SerializeField] private FleetManager shipSpawner;

    // Start is called before the first frame update
    private void Start()
    {
        var randomFleetDB = mapContext.CurrentSystem.RandomFleetDB;
        // enemyShipSpawner.ProjectileParent = projectileParent;
        var fleet = randomFleetDB.fleetDB[Random.Range(0, randomFleetDB.fleetDB.Count)];
        var fleetPositions = fleet.fleetVisualsPrefab.GetComponentsInChildren<Transform>().GetEnumerator();

        foreach (var randomShip in fleet.randomFleet)
            if (fleetPositions.MoveNext())
            {
                var shipData = randomShip.RandomShip();
                var ship = shipSpawner.SpawnShip(shipData, Vector2.zero);
                var spawnPos = fleetPositions.Current as Transform;
                if (spawnPos != null) ship.transform.localPosition = spawnPos.position;
            }
    }
}
