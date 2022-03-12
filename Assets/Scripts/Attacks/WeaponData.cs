using System;
using Attacks;
using Systems;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
    public class WeaponData : UniqueId
    {
        [Header("Info")] [SerializeField] private GameObject turret;
        [SerializeField] private AttackData attackData;

        [Header("Stats")] [SerializeField] private float health;
        [SerializeField] private float range;

        public GameObject Turret => turret;
        public AttackData AttackData => attackData;
        public float BaseHealth => health;
        public float BaseRange => range;
    }
}