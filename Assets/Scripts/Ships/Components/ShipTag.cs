using Ships.DataManagement;
using UnityEngine;

namespace Ships.Components
{
    [AddComponentMenu("")]
    public class ShipTag : MonoBehaviour, IInitializableComponent
    {
        [SerializeField] private ShipList factionShipList;

        private void OnDestroy()
        {
            factionShipList.AddShip(gameObject);
        }

        public void Initialize(ShipData data, ShipSpawner spawner)
        {
            factionShipList = spawner.FactionShipList;
            factionShipList.AddShip(gameObject);
        }
    }
}