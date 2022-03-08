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
        public float FireTimePercent { get; }
        public bool DoAttack(DamageableComponentInfo target);
        public void SetParent(Transform parent);
    }

    [Serializable]
    public abstract class AttackData : UniqueId
    {
        [Header("Info")] [SerializeField] private GameObject turret;
        [SerializeField] private GameObject mapIcon;
        [SerializeField] private Sprite infoWindowSprite;
        [SerializeField] private string attackName;
        [SerializeField] private int cost;

        [Header("Projectile Stats")] [SerializeField]
        private float hitChance;

        [SerializeField] private int damage;
        [SerializeField] private float mapSpeed;
        [SerializeField] private float infoWindowSpeed;

        [Header("Turret Stats")]
        [SerializeField] private float health;
        [SerializeField] private float fireDelay;

        [Header("InfoWindow Animations")] [SerializeField]
        private GameObject hitAnimation;

        [SerializeField] private GameObject missAnimation;
        [SerializeField] private GameObject fireAnimation;

        public GameObject Turret => turret;
        public GameObject MapIcon => mapIcon;
        public Sprite InfoWindowSprite => infoWindowSprite;
        public string AttackName => attackName;
        public int Cost => cost;
        public float BaseHitChance => hitChance;
        public float BaseHealth => health;
        public float BaseDamage => damage;
        public float BaseMapSpeed => mapSpeed;
        public float BaseInfoWindowSpeed => infoWindowSpeed;
        public float FireDelay => fireDelay;
        public GameObject HitAnimation => hitAnimation;
        public GameObject MissAnimation => missAnimation;
        public GameObject FireAnimation => fireAnimation;

        public abstract AttackCommand MakeAttack(DamageableComponentInfo componentInfo);
    }
}