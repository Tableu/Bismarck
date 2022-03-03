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
            return new Attack(componentInfo, this);
        }

        private class Attack : AttackCommand
        {
            private DamageableComponentInfo _componentInfo;
            private AttackData _data;
            private float _fireTime;
            private Transform _parent;

            public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);
            public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);
            public float FireTimePercent => Mathf.Min((Time.fixedTime - _fireTime) / _data.FireDelay, 1);

            public Attack(DamageableComponentInfo componentInfo, AttackData attackData)
            {
                _componentInfo = componentInfo;
                _data = attackData;
                _fireTime = Time.fixedTime;
                HitChanceMultiplier.UpdateBaseValue(attackData.BaseHitChance);
                MaxHealth.UpdateBaseValue(attackData.BaseHealth);
            }

            public void SetParent(Transform parent)
            {
                _parent = parent;
            }

            public bool DoAttack(DamageableComponentInfo target)
            {
                if (target != null)
                {
                    if (Time.fixedTime - _fireTime > _data.FireDelay)
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
                AttackInfo attackInfo = new AttackInfo(_data, target, _data.BaseDamage, HitChanceMultiplier);
                GameObject mapIcon = Instantiate(_data.MapIcon, _componentInfo.ShipInfo.MapIcon.transform.position,
                    Quaternion.identity);
                AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
                attackIcon.AttackInfo = attackInfo;
                attackIcon.Attacker = _componentInfo;
                attackIcon.Target = target;
            }
        }
    }
}