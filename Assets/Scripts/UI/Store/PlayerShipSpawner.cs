using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)][Serializable]
public class PlayerShipSpawner : SpawnerScriptableObject
{
    private List<ShipData> _shipDatas;
    public List<ShipData> FleetData => _shipDatas;
    public PlayerShipDictionary ShipDictionary;

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
        GameObject ship = Instantiate(data.ShipVisuals, parent, false);
        if (ship != null)
        {
            _shipDatas.Add(data);
            ShipDictionary.AddShip(data,ship.GetInstanceID());
            ship.AddComponent(data.BattleController.GetType());
            ShipDataComponent dataComponent = ship.AddComponent<ShipDataComponent>();
            if (dataComponent != null)
                dataComponent.ShipData = data;
        }
    }
}
