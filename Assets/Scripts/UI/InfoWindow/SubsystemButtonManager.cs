using System;
using Ships.Components;
using Ships.Fleets;
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

        public void Refresh(ShipStats target)
        {
            Player.Fleet.AgroStatusMap.TryGetValue(target.Fleet, out FleetAgroStatus fleetAgroStatus);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            DamageableComponent[] damageableComponents = target.GetComponentsInChildren<DamageableComponent>();

            foreach (DamageableComponent damageableComponent in damageableComponents)
            {
                if ((damageableComponent.Subsystem & target.Data.TargetableSubsystems) != Subsystem.None)
                {
                    GameObject subsystemButton = Instantiate(_subsystemButtonPrefab, transform, false);
                    SubsystemButton button = subsystemButton.GetComponent<SubsystemButton>();
                    button.Subsystem = damageableComponent.Subsystem;
                    button.Ship = target;
                    button.ButtonData =
                        _subsystemButtonData.ButtonData.Find(data => data.Subsystem == damageableComponent.Subsystem);
                    button.SubsystemComponent = damageableComponent;
                    button.Player = Player;
                }
            }
        }
    }
}