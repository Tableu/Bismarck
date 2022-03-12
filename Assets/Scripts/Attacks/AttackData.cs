using System;
using Ships.Components;
using Systems.Abilities;
using UnityEngine;

namespace Attacks
{
    [Serializable]
    public abstract class AttackData : AbilityData
    {
        [Header("Info")] [SerializeField] private GameObject mapIcon;
        [SerializeField] private Sprite infoWindowSprite;
        [SerializeField] private string attackName;
        [SerializeField] private int cost;

        [Header("Projectile Stats")] [SerializeField]
        private float hitChance;

        [SerializeField] private float moduleHitChance;
        [SerializeField] private int damage;
        [SerializeField] private float moduleDamagePercent;
        [SerializeField] private float mapSpeed;
        [SerializeField] private float infoWindowSpeed;

        [Header("InfoWindow Animations")] [SerializeField]
        private GameObject hitAnimation;

        [SerializeField] private GameObject missAnimation;
        [SerializeField] private GameObject fireAnimation;

        public GameObject MapIcon => mapIcon;
        public Sprite InfoWindowSprite => infoWindowSprite;
        public string AttackName => attackName;
        public int Cost => cost;
        public float BaseHitChance => hitChance;
        public float BaseModuleHitChance => moduleHitChance;

        public float BaseDamage => damage;
        public float ModuleDamagePercent => moduleDamagePercent;
        public float BaseMapSpeed => mapSpeed;
        public float BaseInfoWindowSpeed => infoWindowSpeed;
        public GameObject HitAnimation => hitAnimation;
        public GameObject MissAnimation => missAnimation;
        public GameObject FireAnimation => fireAnimation;

        public abstract Attack MakeAttack(DamageableComponent component);
    }
}