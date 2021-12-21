using System;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)][Serializable]
public class ShipSpawner : SpawnerScriptableObject
{
    public ShipDictionary ShipDictionary;

    public override void SpawnFleet(List<ShipData> shipDatas, Transform parent)
    {
        foreach (ShipData ship in shipDatas)
        {
            SpawnShip(ship, parent);
        }
    }

    public override void SpawnShip(ShipData data, Transform parent, Vector3? startPos = null)
    {
        if (parent == null)
            return;
        GameObject ship = Instantiate(data.ShipPrefab, parent, false);
        if (ship != null)
        {
            if (startPos != null)
            {
                ship.transform.position = startPos.Value;
                data.StartingPos = ship.transform.localPosition;
            }
            ShipDictionary.AddShip(data,ship.GetInstanceID());
        }
    }
}
