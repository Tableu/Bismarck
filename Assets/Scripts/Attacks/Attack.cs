using Ships.Components;
using Systems.Abilities;
using Systems.Modifiers;
using UI.InfoWindow;
using UnityEngine;

namespace Attacks
{
    public class Attack : CooldownAbility
    {
        private AttackData _data;
        private Transform _parent;
        private DamageableComponent _weapon;
        private DamageableComponent _target;

        public AttackData Data => _data;
        public override ButtonData ButtonData => _data.ButtonData;

        public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);
        public ModifiableStat ModuleHitChanceMultiplier { get; } = new ModifiableStat(0);

        public Attack(AttackData attackData, DamageableComponent weapon)
        {
            _data = attackData;
            _weapon = weapon;
            HitChanceMultiplier.UpdateBaseValue(attackData.BaseHitChance);
            ModuleHitChanceMultiplier.UpdateBaseValue(attackData.BaseModuleHitChance);
            CooldownMultiplier.UpdateBaseValue(attackData.Cooldown);
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public void SetTarget(DamageableComponent target)
        {
            _target = target;
        }

        public override bool Fire(ShipStats attacker)
        {
            if (_target != null)
            {
                if (!OnCooldown)
                {
                    AttackProjectile attackProjectile = _target is Hull
                        ? new AttackProjectile(_data, _target, _data.BaseDamage, HitChanceMultiplier)
                        : new AttackProjectile(_data, _target, _data.BaseDamage, ModuleHitChanceMultiplier);

                    GameObject mapIcon = Object.Instantiate(_data.MapIcon, attacker.transform.position,
                        Quaternion.identity);
                    AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
                    attackIcon.AttackProjectile = attackProjectile;
                    attackIcon.Target = _target;
                    attackIcon.Attacker = _weapon;

                    return true;
                }
            }

            return false;
        }
    }
}