using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ships.DataManagment
{
    [CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipData")]
    [Serializable]
    public class ShipData : ScriptableObject
    {
        public string shipName;
        public GameObject prefab;
        public int Cost;
        public List<AttackScriptableObject> Weapons;
        [SerializeField] private bool blocksMovement = true;

        [Header("Base Stats")] [SerializeField]
        private float health = 10;

        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private float damageMultiplier = 1;
        [SerializeField] private float sensorRange = 50;

        [Header("Config")] [SerializeField] private float targetRange;
        public float BaseHealth => health;
        public float BaseSpeedMultiplier => speedMultiplier;
        public float BaseDamageMultiplier => damageMultiplier;
        public float TargetRange => targetRange;
        public bool BlocksMovement => blocksMovement;
        public float SensorRange => sensorRange;
    }
}