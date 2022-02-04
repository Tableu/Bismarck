using System;
using System.Collections.Generic;
using Systems;
using Systems.Modules;
using UnityEngine;

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
        [SerializeField] private int cost;
        [SerializeField] private List<AttackScriptableObject> weapons;
        [SerializeField] private int moduleGridHeight;
        [SerializeField] private int moduleGridWidth;
        [SerializeField] private List<Module> moduleList;
        [SerializeField] private bool blocksMovement = true;

        [Header("Base Stats")]
        [SerializeField] private float health = 10;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private float damageMultiplier = 1;
        [SerializeField] private float sensorRange = 50;

        [Header("Config")]
        [SerializeField] private float targetRange;
        public float BaseHealth => health;
        public float BaseSpeedMultiplier => speedMultiplier;
        public float BaseDamageMultiplier => damageMultiplier;
        public float TargetRange => targetRange;
        public bool BlocksMovement => blocksMovement;
        public float SensorRange => sensorRange;
        public string DisplayName => displayName;
        public GameObject Visuals => visuals;
        public int Cost => cost;
        public List<AttackScriptableObject> Weapons => weapons;
        public int ModuleGridHeight => moduleGridHeight;
        public int ModuleGridWidth => moduleGridWidth;
        public List<Module> ModuleList => moduleList;
    }
}
