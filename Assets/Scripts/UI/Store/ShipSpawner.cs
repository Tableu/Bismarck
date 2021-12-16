using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)]
public class ShipSpawner : SpawnerScriptableObject
{
    public bool isStore;
    public override ISpawner MakeSpawner(Transform fleetParent)
    {
        return new Spawner(fleetParent, isStore);
    }

    private class Spawner : ISpawner
    {
        private Transform _fleetParent;
        private List<ShipData> _shipDatas;
        private bool _isStore;
        public List<ShipData> FleetData => _shipDatas;

        public Spawner(Transform fleetParent, bool isStore)
        {
            _fleetParent = fleetParent;
            _isStore = isStore;
        }

        public void SpawnFleet(List<ShipData> shipDatas)
        {
            foreach (ShipData ship in shipDatas)
            {
                SpawnShip(ship);
            }
        }

        public void SpawnShip(ShipData data)
        {
            if (_fleetParent == null)
                return;
            GameObject ship = Instantiate(data.ShipVisuals, _fleetParent, false);
            if (ship != null)
            {
                _shipDatas.Add(data);
                ShipController shipController;
                if (!_isStore)
                {
                    shipController = ship.AddComponent(data.BattleController.GetType()) as ShipController;
                }
                else
                {
                    shipController = ship.AddComponent(data.ShopUI.GetType()) as ShipController;
                }

                if (shipController != null)
                    shipController.Init(data);
            }
        }
    }
}
