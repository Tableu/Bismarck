using Attacks;
using Systems.Abilities;
using Systems.Modifiers;
using UnityEngine;
using Weapons;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Exposes data from an attack and provides access attack logic to Player UI and Enemies.
    /// </summary>
    public class Weapon : DamageableComponent
    {
        private WeaponData _weaponData;
        private Attack _attackCommand;
        private DamageableComponent _target;

        public ModifiableStat MaxRange { get; } = new ModifiableStat(0);
        public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);

        public Attack Attack => _attackCommand;

        public void Initialize(ShipStats shipStats, WeaponData weaponData)
        {
            var parent = GameObject.FindWithTag(tag) ?? new GameObject
            {
                tag = tag
            };

            Stats = shipStats;
            _weaponData = weaponData;
            _attackCommand = _weaponData.AttackData.MakeAttack(this);
            _attackCommand.SetParent(parent.transform);
            StartCoroutine(_attackCommand.CooldownTimer());
            SetData(_weaponData.BaseHealth, shipStats.DodgeChanceMultiplier, Subsystem.Weapon);
            MaxRange.UpdateBaseValue(_weaponData.BaseRange);
            MaxHealth.UpdateBaseValue(_weaponData.BaseHealth);
        }

        public bool InRange()
        {
            if (_target != null && (transform.position - _target.transform.position).magnitude < MaxRange)
            {
                return true;
            }

            return false;
        }
    }
}