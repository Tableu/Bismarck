using System;
using System.Collections;
using Attacks;
using Ships.Components;
using Ships.Fleets;
using Systems.Modifiers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Abilities
{
    public class Ability : CooldownAbility
    {
        private AbilityData _data;
        private Transform _parent;
        private ShipStats _user;
        private DamageableComponent _target;

        public AbilityData Data => _data;
        public DamageableComponent Target => _target;
        public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat ModuleHitChanceMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat MaxRange { get; } = new ModifiableStat(0);

        public Ability(AbilityData data, ShipStats user)
        {
            _user = user;
            _data = data;
            HitChanceMultiplier.UpdateBaseValue(_data.BaseHitChance);
            ModuleHitChanceMultiplier.UpdateBaseValue(_data.BaseModuleHitChance);
            MaxRange.UpdateBaseValue(data.BaseRange);
            CooldownMultiplier.UpdateBaseValue(data.Cooldown);
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public void SetTarget(DamageableComponent target)
        {
            _target = target;
        }

        public bool InRange()
        {
            if (_target != null && (_user.transform.position - _target.transform.position).magnitude < MaxRange)
            {
                return true;
            }

            return false;
        }

        public bool TargetingSelf()
        {
            if (_target == null && _data.ValidTargets.HasFlag(FleetAgroStatus.Self))
            {
                return true;
            }

            return false;
        }

        public override bool Fire()
        {
            if (!OnCooldown && (TargetingSelf() || InRange()))
            {
                if (_data.FireAnimation == null && _data.HitAnimation == null && _data.MissAnimation == null)
                {
                    foreach (ModifierData modifier in Data.Modifiers)
                    {
                        modifier.AttachNewModifer(_user);
                    }
                }
                else if (_target != null)
                {
                    AttackProjectile attackProjectile = _target is Hull
                        ? new AttackProjectile(_data, _target, _data.BaseDamage, HitChanceMultiplier)
                        : new AttackProjectile(_data, _target, _data.BaseDamage, ModuleHitChanceMultiplier);

                    GameObject mapIcon = Object.Instantiate(_data.MapIcon, _user.transform.position,
                        Quaternion.identity);
                    AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
                    attackIcon.AttackProjectile = attackProjectile;
                    attackIcon.Target = _target;
                    attackIcon.Attacker = _user;
                    return true;
                }
            }

            return false;
        }
    }

    public abstract class CooldownAbility
    {
        private bool _onCooldown;
        public bool OnCooldown => _onCooldown;
        public ModifiableStat CooldownMultiplier { get; } = new ModifiableStat(0);
        public IEnumerator CooldownTimer()
        {
            float startTime = Time.time;
            _onCooldown = true;
            while (Time.time - startTime < CooldownMultiplier)
            {
                float percentage = Mathf.Min((Time.time - startTime) / CooldownMultiplier, 1);
                CooldownEvent?.Invoke(percentage);
                yield return null;
            }

            _onCooldown = false;
        }

        public abstract bool Fire();

        public Action<float> CooldownEvent;
    }
}