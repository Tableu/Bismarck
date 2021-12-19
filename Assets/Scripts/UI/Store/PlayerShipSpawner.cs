using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)][Serializable]
public class PlayerShipSpawner : SpawnerScriptableObject
{
    public Transform _fleetParent;
    private List<ShipData> _shipDatas;
    public List<ShipData> FleetData => _shipDatas;

    public override void SpawnFleet(List<ShipData> shipDatas)
    {
        foreach (ShipData ship in shipDatas)
        {
            SpawnShip(ship);
        }
    }

    public override void SpawnShip(ShipData data)
    {
        if (_fleetParent == null)
            return;
        GameObject ship = Instantiate(data.ShipVisuals, _fleetParent, false);
        if (ship != null)
        {
            _shipDatas.Add(data);
            ShipController shipController;
            shipController = ship.AddComponent(data.BattleController.GetType()) as ShipController;

            if (shipController != null)
                shipController.Init(data);
        }
    }
}
