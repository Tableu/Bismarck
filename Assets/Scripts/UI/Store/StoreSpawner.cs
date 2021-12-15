using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/StoreSpawner", order = 0)]
public class StoreSpawner : ScriptableObject, IFleetSpawner
{
    public Transform fleet;
    public LayerMask layer;
    private List<ShipData> _shipDatas;
    public List<ShipData> FleetData => _shipDatas;
    
    public void SpawnFleet(List<ShipData> shipDatas)
    {
        foreach (ShipData ship in shipDatas)
        {
            SpawnShip(ship);
        }
    }

    public void SpawnShip(ShipData data)
    { 
        GameObject ship = Instantiate(data.shipPrefab, fleet, false);
        ship.transform.localPosition = data.startingPos;
        if (ship != null)
        { 
            _shipDatas.Add(data);
            ship.layer = layer;
        }
    }
}

