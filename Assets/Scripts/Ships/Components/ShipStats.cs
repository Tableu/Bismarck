using System;
using Scene;
using Ships.DataManagement;
using Ships.Fleets;
using Systems.Modifiers;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores basic ship information, such as stats and data.
    /// </summary>
    public class ShipStats : ModifiableTarget
    {
        private GameObject _visuals;
        private AbilityManager _abilityManager;
        private TargetingHelper _targetingHelper;
        [SerializeField] private ShipData data;
        public ShipData Data => data;
        public GameObject Visuals => _visuals;
        public AbilityManager AbilityManager => _abilityManager;
        public TargetingHelper TargetingHelper => _targetingHelper;
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

            //Init hull
            Hull hull = gameObject.AddComponent<Hull>();
            hull.SetData(data.BaseHealth, data.BaseDodgeChance, Subsystem.Hull);

            //Init AbilityManager
            _abilityManager = gameObject.AddComponent<AbilityManager>();
            _abilityManager.Initialize(data, this);

            _targetingHelper = gameObject.AddComponent<TargetingHelper>();
            _targetingHelper.Initialize(data, this);
        }

        public event Action OnShipDestroyed;
    }
}
