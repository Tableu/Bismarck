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
        public ShipInfo Player;
        [SerializeField] private GameObject _subsystemButtonPrefab;
        [SerializeField] private SubsystemButtonData _subsystemButtonData;

        public void Refresh(ShipInfo shipInfo)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            DamageableComponentInfo[] targets = shipInfo.GetComponentsInChildren<DamageableComponentInfo>();

            foreach (DamageableComponentInfo target in targets)
            {
                if ((target.Subsystem & shipInfo.Data.TargetableSubsystems) != Subsystem.None)
                {
                    GameObject subsystemButton = Instantiate(_subsystemButtonPrefab, transform, false);
                    SubsystemButton button = subsystemButton.GetComponent<SubsystemButton>();
                    button.Subsystem = target.Subsystem;
                    button.ShipInfo = shipInfo;
                    button.ButtonData =
                        _subsystemButtonData.ButtonData.Find(data => data.Subsystem == target.Subsystem);
                    button.Target = target;
                    button.Player = Player;
                }
            }
        }
    }
}