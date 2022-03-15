using System;
using Systems;
using Systems.Abilities;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
    public class WeaponData : UniqueId
    {
        [Header("Info")] [SerializeField] private GameObject turret;
        [SerializeField] private AbilityData abilityData;

        [Header("Stats")] [SerializeField] private float health;

        public GameObject Turret => turret;
        public AbilityData AttackData => abilityData;
        public float BaseHealth => health;
    }
}