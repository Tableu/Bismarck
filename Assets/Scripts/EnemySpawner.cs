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
        foreach (RandomShipList randomShip in fleet.randomFleet)
        {
            ShipData shipData = randomShip.RandomShip();
            enemyShipSpawner.SpawnShip(shipData, fleetParent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
