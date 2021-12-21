using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)][Serializable]
public class PlayerShipSpawner : SpawnerScriptableObject
{
    public ShipDictionary ShipDictionary;

    public override void SpawnFleet(List<ShipData> shipDatas, Transform parent)
    {
        foreach (ShipData ship in shipDatas)
        {
            SpawnShip(ship, parent);
        }
    }

    public override void SpawnShip(ShipData data, Transform parent)
    {
        if (parent == null)
            return;
        GameObject ship = Instantiate(data.ShipPrefab, parent, false);
        if (ship != null)
        {
            ShipDictionary.AddShip(data,ship.GetInstanceID());
        }
    }
}
