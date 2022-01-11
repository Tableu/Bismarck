using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ShipDictionary enemyShipDict;
    [SerializeField] private ShipSpawner enemyShipSpawner;
    [SerializeField] private FleetDBScriptableObject randomFleetDB;
    [SerializeField] private Transform fleetParent;
    [SerializeField] private Transform projectileParent;
    // Start is called before the first frame update
    void Start()
    {
        enemyShipSpawner.ProjectileParent = projectileParent;
        RandomFleet fleet = randomFleetDB.fleetDB[0];
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
