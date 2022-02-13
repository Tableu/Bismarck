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
        Engine = 1 << 1,
        Hull = 1 << 2
    }

    public class SubsystemButtonManager : MonoBehaviour
    {
        private ShipInfo _info;
        [SerializeField] private GameObject _subsystemButtonPrefab;
        [SerializeField] private SubsystemButtonData _subsystemButtonData;

        void Start()
        {
            _info = GetComponentInParent<ShipInfo>();
            if (_info == null)
            {
                return;
            }

            IDamageable[] targets = _info.gameObject.GetComponents<IDamageable>();
            foreach (IDamageable target in targets)
            {
                if ((target.Subsystem & _info.Data.EnabledSubsystems) != Subsystem.None)
                {
                    GameObject subsystemButton = Instantiate(_subsystemButtonPrefab, transform, false);
                    SubsystemButton button = subsystemButton.GetComponent<SubsystemButton>();
                    button.Subsystem = target.Subsystem;
                    button.ShipInfo = _info;
                    button.ButtonData =
                        _subsystemButtonData.ButtonData.Find(data => data.Subsystem == target.Subsystem);
                    button.Target = target;
                }
            }
        }
    }
}