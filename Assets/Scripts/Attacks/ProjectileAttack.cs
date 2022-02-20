using Ships.Components;
using Systems.Modifiers;
using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Projectile", order = 0)]
    public class ProjectileAttack : AttackData
    {
        public override AttackCommand MakeAttack(ShipInfo shipInfo)
        {
            return new Attack(shipInfo, MapIcon, FireDelay, BaseHitChance, BaseDamage, BaseHealth);
        }

        private class Attack : AttackCommand
        {
            private ShipInfo _shipInfo;
            private float _fireDelay;
            private float _fireTime;
            private float _damage;
            private Transform _parent;
            private GameObject _mapIcon;

            public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);
            public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);

            public Attack(ShipInfo shipInfo, GameObject mapIcon, float fireDelay, float hitChance, float damage,
                float health)
            {
                _shipInfo = shipInfo;
                _mapIcon = mapIcon;
                _fireDelay = fireDelay;
                _fireTime = Time.deltaTime;
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
                    if (Time.deltaTime - _fireTime > _fireDelay)
                    {
                        _fireTime = Time.deltaTime;
                        SpawnProjectile(target);
                        return true;
                    }
                }

                return false;
            }

            private void SpawnProjectile(DamageableComponentInfo target)
            {
                Damage damage = new Damage(target, _damage, HitChanceMultiplier);
                GameObject mapIcon = Instantiate(_mapIcon, _shipInfo.transform.position, Quaternion.identity, _parent);
                AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
                attackIcon.Damage = damage;
                attackIcon.Attacker = _shipInfo;
                attackIcon.Target = target.ShipInfo;
            }
        }
    }
}