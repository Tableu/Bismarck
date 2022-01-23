using Ships.DataManagement;
using UnityEngine;

namespace Ships.Components
{
    [AddComponentMenu("")]
    public class ShipTags : MonoBehaviour, IInitializableComponent
    {
        private ShipList factionShips;
        public ShipSpawner ShipSpawner { get; private set; }

        private void OnDestroy()
        {
            factionShips.RemoveShip(gameObject);
        }

        public void Initialize(ShipData data, ShipSpawner spawner)
        {
            factionShips = spawner.FactionShipList;
            ShipSpawner = spawner;
            factionShips.AddShip(gameObject);
        }
    }
}