using Ships.Components;
using Systems.Modifiers;
using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Projectile", order = 0)]
    public class ProjectileAttack : AttackData
    {
        public override AttackCommand MakeAttack(DamageableComponentInfo componentInfo)
        {
            return new Attack(componentInfo, MapIcon, FireDelay, BaseHitChance, BaseDamage, BaseHealth);
        }

        private class Attack : AttackCommand
        {
            private DamageableComponentInfo _componentInfo;
            private float _fireDelay;
            private float _fireTime;
            private float _damage;
            private Transform _parent;
            private GameObject _mapIcon;

            public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);
            public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);
            public float FireTimePercent => Mathf.Min((Time.fixedTime - _fireTime) / _fireDelay, 1);

            public Attack(DamageableComponentInfo componentInfo, GameObject mapIcon, float fireDelay, float hitChance,
                float damage,
                float health)
            {
                _componentInfo = componentInfo;
                _mapIcon = mapIcon;
                _fireDelay = fireDelay;
                _fireTime = Time.fixedTime;
                _damage = damage;
                HitChanceMultiplier.UpdateBaseValue(hitChance);
                MaxHealth.UpdateBaseValue(health);
            }

            public void SetParent(Transform parent)
            {
                _parent = parent;
            }

            public bool DoAttack(DamageableComponentInfo target)
            {
                if (target != null)
                {
                    if (Time.fixedTime - _fireTime > _fireDelay)
                    {
                        _fireTime = Time.fixedTime;
                        SpawnProjectile(target);
                        return true;
                    }
                }

                return false;
            }

            private void SpawnProjectile(DamageableComponentInfo target)
            {
                Damage damage = new Damage(target, _damage, HitChanceMultiplier);
                GameObject mapIcon = Instantiate(_mapIcon, _componentInfo.ShipInfo.MapIcon.transform.position,
                    Quaternion.identity);
                AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
                attackIcon.Damage = damage;
                attackIcon.Attacker = _componentInfo;
                attackIcon.Target = target;
            }
        }
    }
}