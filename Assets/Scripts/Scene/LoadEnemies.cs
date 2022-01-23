using System.Collections;
using StarMap;
using UnityEngine;

public class LoadEnemies : MonoBehaviour
{
    [SerializeField] private ShipSpawner enemyShipSpawner;
    [SerializeField] private MapContext mapContext;
    [SerializeField] private Transform fleetParent;
    [SerializeField] private Transform projectileParent;
    // Start is called before the first frame update
    void Start()
    {
        FleetDBScriptableObject randomFleetDB = mapContext.CurrentSystem.RandomFleetDB;
        enemyShipSpawner.ProjectileParent = projectileParent;
        RandomFleet fleet = randomFleetDB.fleetDB[Random.Range(0, randomFleetDB.fleetDB.Count)];
        IEnumerator fleetPositions = fleet.fleetVisualsPrefab.GetComponentsInChildren<Transform>().GetEnumerator();
        
        foreach (RandomShipList randomShip in fleet.randomFleet)
        {
            if (fleetPositions.MoveNext())
            {
                ShipData shipData = randomShip.RandomShip();
                GameObject ship = enemyShipSpawner.SpawnShip(shipData, fleetParent);
                Transform spawnPos = fleetPositions.Current as Transform;
                if (spawnPos != null)
                {
                    ship.transform.localPosition = spawnPos.position;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
