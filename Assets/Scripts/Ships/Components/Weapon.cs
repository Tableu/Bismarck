using Attacks;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Exposes data from an attack and provides access attack logic to Player UI and Enemies.
    /// </summary>
    public class Weapon : DamageableComponentInfo
    {
        private AttackData _attackData;
        private AttackCommand _attackCommand;
        private DamageableComponentInfo _target;
        
        public float FireTimePercent => _attackCommand.FireTimePercent;

        public void Initialize(ShipInfo shipInfo, AttackData attackData)
        {
            var parent = GameObject.FindWithTag(tag) ?? new GameObject
            {
                tag = tag
            };

            _info = shipInfo;
            _attackCommand = attackData.MakeAttack(this);
            _attackCommand.SetParent(parent.transform);
            SetData(attackData.BaseHealth, shipInfo.DodgeChanceMultiplier, Subsystem.Weapon);
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
    }
}