using System;
using System.Collections.Generic;
using Attacks;
using Scene;
using Ships.DataManagement;
using Ships.Fleets;
using SystemMap;
using Systems.Abilities;
using Systems.Modifiers;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores basic ship information, such as stats and data.
    /// </summary>
    public class ShipInfo : ModifiableTarget
    {
        private GameObject _visuals;
        private GameObject _mapIcon;
        private List<Weapon> _weapons = new List<Weapon>();
        private List<Ability> _abilities = new List<Ability>();
        [SerializeField] private ShipData data;
        public ShipData Data => data;
        public GameObject Visuals => _visuals;
        public GameObject MapIcon => _mapIcon;
        public List<Ability> Abilities => _abilities;
        public FleetManager Fleet { get; private set; }

        // ModifiableStat must be read only so that other components can get references to them during Start/Awake.
        public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);
        public ModifiableStat DamageMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat SpeedMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat DodgeChanceMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat RepairTimeMultiplier { get; } = new ModifiableStat(0);

        private void Awake()
        {
            if (data != null)
            {
                Initialize();
                InitializeHull();
                InitializeWeapons();
                InitializeAbilities();
            }
        }

        private void OnDestroy()
        {
            OnShipDestroyed?.Invoke();
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
            InitializeHull();
            InitializeWeapons();
            InitializeAbilities();
        }

        [ContextMenu("InitializeWeapons")]
        public void InitializeWeapons()
        {
            foreach (AttackData attackData in data.Weapons)
            {
                GameObject turret = Instantiate(attackData.Turret, _visuals.transform);
                turret.transform.position = _visuals.transform.position;
                Weapon weapon = gameObject.AddComponent<Weapon>();
                weapon.Initialize(this, attackData);
                _weapons.Add(weapon);
            }
        }

        [ContextMenu("InitializeWeapons")]
        public void InitializeAbilities()
        {
            foreach (AbilityData abilityData in data.Abilities)
            {
                Ability ability = new Ability(abilityData);
                _abilities.Add(ability);
            }
        }

        [ContextMenu("InitializeHull")]
        public void InitializeHull()
        {
            Hull hull = gameObject.AddComponent<Hull>();
            hull.SetData(data.BaseHealth, data.BaseDodgeChance, Subsystem.Hull);
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
            RepairTimeMultiplier.UpdateBaseValue(data.BaseRepairTime);

            // Add ship visuals
            _visuals = Instantiate(data.Visuals, ShipVisualsManager.Instance.GetParent());
            _visuals.transform.position = ShipVisualsManager.Instance.GetPosition();
            _mapIcon = Instantiate(data.MapIcon, transform);
            MapIcon mapIcon = _mapIcon.AddComponent<MapIcon>();
            mapIcon.cam = Camera.main;
            mapIcon.Init();
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

        public event Action OnShipDestroyed;

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
