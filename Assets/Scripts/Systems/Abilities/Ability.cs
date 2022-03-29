using System;
using System.Collections;
using Attacks;
using Ships.Components;
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
        private DamageableComponent _damageableComponent;

        public AbilityData Data => _data;
        public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat ModuleHitChanceMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat MaxRange { get; } = new ModifiableStat(0);

        public Ability(AbilityData data, ShipStats user, DamageableComponent damageableComponent = null)
        {
            _user = user;
            _data = data;
            _damageableComponent = damageableComponent;
            HitChanceMultiplier.UpdateBaseValue(_data.BaseHitChance);
            ModuleHitChanceMultiplier.UpdateBaseValue(_data.BaseModuleHitChance);
            MaxRange.UpdateBaseValue(data.BaseRange);
            CooldownMultiplier.UpdateBaseValue(data.Cooldown);
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public override bool Fire()
        {
            if (!OnCooldown)
            {
                if (_user.TargetingHelper.TargetingSelf(this))
                {
                    foreach (ModifierData modifier in Data.Modifiers)
                    {
                        modifier.AttachNewModifer(_user);
                    }

                    return true;
                }
                else if (_user.TargetingHelper.InRange(this))
                {
                    AttackProjectile attackProjectile = _user.TargetingHelper.Target is Hull
                        ? new AttackProjectile(_data, _user.TargetingHelper.Target, _data.BaseDamage,
                            HitChanceMultiplier)
                        : new AttackProjectile(_data, _user.TargetingHelper.Target, _data.BaseDamage,
                            ModuleHitChanceMultiplier);

                    GameObject mapIcon = Object.Instantiate(_data.MapIcon, _user.transform.position,
                        Quaternion.identity);
                    mapIcon.transform.parent = _parent;
                    AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
                    attackIcon.AttackProjectile = attackProjectile;
                    attackIcon.Target = _user.TargetingHelper.Target;
                    attackIcon.Attacker = _user;
                    if (_damageableComponent != null)
                    {
                        attackIcon.SpawnLocation = _damageableComponent.Visuals.transform.position;
                    }
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