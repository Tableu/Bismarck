using System;
using System.Collections.Generic;
using Systems;
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
        [SerializeField] [Range(0.001f, 1f)] private float cameraSizeMultiplier = 1f;
        [SerializeField] [Range(0.001f, 1f)] private float backgroundSizeMultiplier = 1f;
        [SerializeField] private int cost;
        [SerializeField] private List<AttackScriptableObject> weapons;
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
        public float CameraSizeMultiplier => cameraSizeMultiplier;
        public float BackgroundSizeMultiplier => backgroundSizeMultiplier;
        public int Cost => cost;
        public List<AttackScriptableObject> Weapons => weapons;
    }
}
