using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ShipDictionary enemyShipDict;
    [SerializeField] private ShipSpawner enemyShipSpawner;
    [SerializeField] private FleetDBScriptableObject randomFleetDB;
    [SerializeField] private Transform fleetParent;
    // Start is called before the first frame update
    void Start()
    {
        RandomFleet fleet = randomFleetDB.fleetDB[0];
        foreach (RandomShipList randomShip in fleet.randomFleet)
        {
            enemyShipSpawner.SpawnShip(randomShip.RandomShip(), fleetParent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
