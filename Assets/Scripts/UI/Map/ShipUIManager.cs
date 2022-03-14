using Ships.Components;
using UI.Ship;
using UnityEngine;

namespace UI.Map
{
    public class ShipUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject shipIconPrefab;

        private void Start()
        {
            ShipStats[] ships = FindObjectsOfType<ShipStats>();
            foreach (ShipStats ship in ships)
            {
                GameObject shipIcon = Instantiate(shipIconPrefab, parent.transform);
                ShipIcon icon = shipIcon.GetComponent<ShipIcon>();
                if (icon != null)
                {
                    icon.Bind(ship);
                }


                GameObject healthBar = Instantiate(healthBarPrefab, shipIcon.transform);
                HealthBar script = healthBar.GetComponent<HealthBar>();
                if (script != null)
                {
                    script.Bind(ship.GetComponent<Hull>());
                }
            }
        }
    }
}