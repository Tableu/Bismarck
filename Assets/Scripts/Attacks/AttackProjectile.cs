using Ships.Components;
using UnityEngine;

namespace Attacks
{
    public class AttackProjectile
    {
        private DamageableComponent _target;
        private AttackData _attackData;
        private float _rawDamage;
        private float _hitChance;

        public AttackData AttackData => _attackData;
        public float RawDamage => _rawDamage;
        public float HitChance => _hitChance;

        public AttackProjectile(AttackData attackData, DamageableComponent target, float rawDamage, float hitChance)
        {
            _attackData = attackData;
            _target = target;
            _rawDamage = rawDamage;
            _hitChance = hitChance;
        }

        public bool Hit()
        {
            if (_target != null)
            {
                float hit = Random.Range(0f, 1f);
                if (hit > (1 - _hitChance) + _target.DodgeChance)
                {
                    return true;
                }
            }

            return false;
        }

        public void ApplyDamage()
        {
            if (_target != null)
            {
                if (_target is Hull)
                {
                    _target.TakeDamage(_rawDamage);
                }
                else
                {
                    _target.TakeDamage(_rawDamage * Mathf.Min(_attackData.ModuleDamagePercent, 1));
                    Hull hull = _target.GetComponent<Hull>();
                    if (hull != null)
                    {
                        hull.TakeDamage(_rawDamage * (Mathf.Max(0, 1 - _attackData.ModuleDamagePercent)));
                    }
                }
            }
        }
    }
}