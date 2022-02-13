using System;
using Ships.Components;
using UnityEngine;

namespace UI.InfoWindow
{
    [Flags]
    public enum Subsystem
    {
        None = 0,
        Weapon = 1 << 0,
        Engine = 1 << 1
    }

    public class SubsystemButtonManager : MonoBehaviour
    {
        public ShipInfo ShipInfo;
        [SerializeField] private GameObject _subsystemButtonPrefab;
        [SerializeField] private SubsystemButtonData _subsystemButtonData;

        void Start()
        {
            foreach (Subsystem subsystem in Enum.GetValues(typeof(Subsystem)))
            {
                if ((subsystem & ShipInfo.Data.EnabledSubsystems) != Subsystem.None)
                {
                    GameObject subsystemButton = Instantiate(_subsystemButtonPrefab, transform, false);
                    SubsystemButton button = subsystemButton.GetComponent<SubsystemButton>();
                    button.Subsystem = subsystem;
                    button.ShipInfo = ShipInfo;
                    button.ButtonData = _subsystemButtonData.ButtonData.Find(data => data.Subsystem == subsystem);
                }
            }
        }
    }
}