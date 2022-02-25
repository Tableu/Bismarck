using System.Collections.Generic;
using Attacks;
using Ships.DataManagement;
using Ships.Fleets;
using Systems.Modifiers;
using UnityEngine;

namespace Ships.Components
{
    /// <summary>
    ///     Stores basic ship information, such as stats and data.
    /// </summary>
    public class ShipInfo : MonoBehaviour
    {
        private GameObject _visuals;
        private GameObject _mapIcon;
        private List<Weapon> _weapons = new List<Weapon>();
        [SerializeField] private ShipData data;
        public ShipData Data => data;
        public GameObject Visuals => _visuals;
        public GameObject MapIcon => _mapIcon;
        public FleetManager Fleet { get; private set; }

        // ModifiableStat must be read only so that other components can get references to them during Start/Awake.
        public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);
        public ModifiableStat DamageMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat SpeedMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat DodgeChanceMultiplier { get; } = new ModifiableStat(0);

        private void Awake()
        {
            if (data != null)
            {
                Initialize();
            }
        }


        /// <summary>
        ///     Used to initialize the data field when a ship is instantiated at runtime.
        ///     Must be called immediately after instantiation if the data field is not set in the inspector.
        /// </summary>
        /// <param name="shipData">The data to initialize the ship with</param>
        public void Initialize(ShipData shipData)
        {
            // This function is not expected to be called multiple times or if ship data is provided via the inspector
            Debug.Assert(data == null, "ShipInfo.data overwritten");
            data = shipData;
            Initialize();
        }

        [ContextMenu("InitializeWeapons")]
        public void InitializeWeapons()
        {
            foreach (AttackData attackData in data.Weapons)
            {
                GameObject turret = Instantiate(attackData.Turret, transform);
                Weapon weapon = turret.AddComponent<Weapon>();
                weapon.Initialize(this, attackData);
                _weapons.Add(weapon);
            }
        }

        [ContextMenu("InitializeHull")]
        public void InitializeHull()
        {
            Hull hull = gameObject.AddComponent<Hull>();
            hull.Init(data.BaseHealth, data.BaseDodgeChance);
        }

        /// <summary>
        ///     Performs internal initialization, must be called on or before first frame.
        /// </summary>
        private void Initialize()
        {
            var parent = transform.parent;
            Debug.Assert(parent != null, "transform.parent != null");
            Fleet = parent.GetComponent<FleetManager>();
            Debug.Assert(Fleet != null, "Ship Parent must contain fleet manager");
            // Set all the base values
            MaxHealth.UpdateBaseValue(data.BaseHealth);
            DamageMultiplier.UpdateBaseValue(data.BaseDamageMultiplier);
            DodgeChanceMultiplier.UpdateBaseValue(data.BaseDodgeChance);
            SpeedMultiplier.UpdateBaseValue(data.BaseSpeedMultiplier);

            // Add ship visuals
            _visuals = Instantiate(data.Visuals, ShipVisualsManager.Instance.GetParent());
            _visuals.transform.position = ShipVisualsManager.Instance.GetPosition(data.Visuals);
            _mapIcon = Instantiate(data.MapIcon, transform);
        }

        public void SetWeaponsTarget(DamageableComponentInfo target)
        {
            if (target != null)
            {
                foreach (Weapon weapon in _weapons)
                {
                    weapon.SetTarget(target);
                }
            }
        }

#if UNITY_EDITOR
        [Header("Test")]
        public DamageableComponentInfo TestTarget;
        [ContextMenu("Set Weapon Target")]
        private void TestSetWeaponsTarget()
        {
            SetWeaponsTarget(TestTarget);
        }
#endif
    }
}
