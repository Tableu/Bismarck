using System;
using System.Collections.Generic;
using Systems;
using Systems.Abilities;
using Systems.Modules;
using UnityEngine;
using Weapons;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.DataManagement
{
    /// <summary>
    ///     Stores the data that is different between separate ship types (ex. base stats and visuals).
    ///     Also stores a unique ID associated with the ship type to use for saving and loading.
    /// </summary>
    [CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipData")]
    [Serializable]
    public class ShipData : UniqueId
    {
        [Header("Info")]
        [SerializeField] private string displayName;
        [SerializeField] private GameObject visuals;
        [SerializeField] private GameObject mapIcon;
        [SerializeField] private GameObject healthBar;
        [SerializeField] [Range(0.001f, 1f)] private float cameraSizeMultiplier = 1f;
        [SerializeField] private int cost;
        [SerializeField] private List<WeaponData> weapons;
        [SerializeField] private List<AbilityData> abilities;
        [SerializeField] private int moduleGridHeight;
        [SerializeField] private int moduleGridWidth;
        [SerializeField] private List<Module> defaultModules;
        [SerializeField] private Subsystem targetableSubsystems;
        [SerializeField] private bool blocksMovement = true;

        [Header("Base Stats")]
        [SerializeField] private float health = 10;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private float damageMultiplier = 1;
        [SerializeField] private float dodgeChanceMultiplier = 0;
        [SerializeField] private float repairTime;
        [SerializeField] private float sensorRange = 50;

        [Header("Config")]
        [SerializeField] private float targetRange;
        public float BaseHealth => health;
        public float BaseSpeedMultiplier => speedMultiplier;
        public float BaseDamageMultiplier => damageMultiplier;
        public float BaseDodgeChance => dodgeChanceMultiplier;
        public float BaseRepairTime => repairTime;
        public float TargetRange => targetRange;
        public bool BlocksMovement => blocksMovement;
        public float SensorRange => sensorRange;
        public string DisplayName => displayName;
        public GameObject Visuals => visuals;
        public GameObject MapIcon => mapIcon;
        public GameObject HealthBar => healthBar;
        public float CameraSizeMultiplier => cameraSizeMultiplier;
        public int Cost => cost;
        public List<WeaponData> Weapons => weapons;
        public List<AbilityData> Abilities => abilities;
        public int ModuleGridHeight => moduleGridHeight;
        public int ModuleGridWidth => moduleGridWidth;
        public List<Module> DefaultModules => defaultModules;
        public Subsystem TargetableSubsystems => targetableSubsystems;
    }
}
