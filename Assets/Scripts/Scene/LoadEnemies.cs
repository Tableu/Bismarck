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
        enemyShipSpawner.ProjectileParent = projectileParent;
        IEnumerator fleetPositions = mapContext.CurrentSystem.FleetPositions.GetEnumerator();

        foreach (ShipData shipData in mapContext.CurrentSystem.Fleet)
        {
            if (fleetPositions.MoveNext())
            {
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
