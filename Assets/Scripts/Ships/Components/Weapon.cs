using Attacks;
using UnityEngine;

namespace Ships.Components
{
    public class Weapon : DamageableComponentInfo
    {
        private AttackData _attackData;
        private AttackCommand _attackCommand;
        private DamageableComponentInfo _target;
        private bool _disabled;

        public bool Disabled => _disabled;

        public void Initialize(ShipInfo shipInfo, AttackData attackData)
        {
            var parent = GameObject.FindWithTag(tag) ?? new GameObject
            {
                tag = tag
            };

            _attackCommand = attackData.MakeAttack(shipInfo);
            _attackCommand.SetParent(parent.transform);
            Init(attackData.BaseHealth, shipInfo.DodgeChanceMultiplier);
        }

        public void SetTarget(DamageableComponentInfo target)
        {
            _target = target;
        }

        [ContextMenu("Fire")]
        public bool Fire()
        {
            return _attackCommand.DoAttack(_target);
        }

        protected override void DisableComponent()
        {
            if (Health <= 0)
            {
                _disabled = true;
            }
            else if (Health > 100)
            {
                _disabled = false;
            }
        }
    }
}