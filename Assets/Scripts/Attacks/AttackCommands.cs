using System;
using Ships.Components;
using Systems;
using Systems.Modifiers;
using UnityEngine;

namespace Attacks
{
    public interface AttackCommand
    {
        public ModifiableStat HitChanceMultiplier { get; }
        public ModifiableStat MaxHealth { get; }
        public bool DoAttack(DamageableComponentInfo target);
        public void SetParent(Transform parent);
    }

    [Serializable]
    public abstract class AttackData : UniqueId
    {
        [Header("Info")] [SerializeField] private GameObject turret;
        [SerializeField] private GameObject mapIcon;
        [SerializeField] private string attackName;
        [SerializeField] private int cost;

        [Header("Projectile Stats")] [SerializeField]
        private float hitChance;

        [SerializeField] private int damage;

        [Header("Turret Stats")]
        [SerializeField] private float health;
        [SerializeField] private float fireDelay;

        public GameObject Turret => turret;
        public GameObject MapIcon => mapIcon;
        public string AttackName => attackName;
        public int Cost => cost;
        public float BaseHitChance => hitChance;
        public float BaseHealth => health;
        public float BaseDamage => damage;
        public float FireDelay => fireDelay;

        public abstract AttackCommand MakeAttack(ShipInfo shipInfo);
    }
}