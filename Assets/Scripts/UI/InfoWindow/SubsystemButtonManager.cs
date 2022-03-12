using System;
using Ships.Components;
using UnityEngine;

namespace UI.InfoWindow
{
    /// <summary>
    ///     Used as a bitmask and type for IDamageable classes and Subsystem buttons.
    /// </summary>
    [Flags]
    public enum Subsystem
    {
        None = 0,
        Weapon = 1 << 0,
        Engine = 1 << 1,
        Hull = 1 << 2
    }

    /// <summary>
    ///     Instantiates subsystem buttons in a horizontal layout group in a ships Targeting Info Window.
    /// </summary>
    public class SubsystemButtonManager : MonoBehaviour
    {
        public ShipStats Player;
        [SerializeField] private GameObject _subsystemButtonPrefab;
        [SerializeField] private SubsystemButtonData _subsystemButtonData;

        public void Refresh(ShipStats shipStats)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            DamageableComponent[] targets = shipStats.GetComponentsInChildren<DamageableComponent>();

            foreach (DamageableComponent target in targets)
            {
                if ((target.Subsystem & shipStats.Data.TargetableSubsystems) != Subsystem.None)
                {
                    GameObject subsystemButton = Instantiate(_subsystemButtonPrefab, transform, false);
                    SubsystemButton button = subsystemButton.GetComponent<SubsystemButton>();
                    button.Subsystem = target.Subsystem;
                    button.shipStats = shipStats;
                    button.ButtonData =
                        _subsystemButtonData.ButtonData.Find(data => data.Subsystem == target.Subsystem);
                    button.Target = target;
                    button.Player = Player;
                }
            }
        }
    }
}