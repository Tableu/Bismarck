using Ships.Components;
using UI.InfoWindow;
using UnityEngine;

namespace UI.Weapons
{
    /// <summary>
    ///     Instantiates weapon buttons for each Weapon component on the ship. Sets relevant data and references.
    /// </summary>
    public class WeaponButtonManager : MonoBehaviour
    {
        public ShipInfo Player;
        [SerializeField] private GameObject _weaponButtonPrefab;
        [SerializeField] private SubsystemButtonData _weaponButtonData;

        private void Start()
        {
            Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if (Player != null)
            {
                Weapon[] weapons = Player.GetComponentsInChildren<Weapon>();

                foreach (Weapon weapon in weapons)
                {
                    GameObject subsystemButton = Instantiate(_weaponButtonPrefab, transform, false);
                    SubsystemButton button = subsystemButton.GetComponent<SubsystemButton>();
                    button.Subsystem = weapon.Subsystem;
                    button.ShipInfo = Player;
                    button.ButtonData =
                        _weaponButtonData.ButtonData.Find(data => data.Subsystem == weapon.Subsystem);
                    button.Target = weapon;
                    button.Player = Player;
                    weapon.OnDisabledChanged += Refresh;
                }
            }
        }
    }
}