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
        private Ability _attackCommand;
        private DamageableComponent _target;
        
        public ModifiableStat MaxHealth { get; } = new ModifiableStat(0);

        public Ability Attack => _attackCommand;

        public void Initialize(ShipStats shipStats, WeaponData weaponData, GameObject visuals)
        {
            var parent = GameObject.FindWithTag(tag) ?? new GameObject
            {
                tag = tag
            };

            Stats = shipStats;
            _weaponData = weaponData;
            _attackCommand = _weaponData.AttackData.MakeAbility(shipStats, this);
            _attackCommand.SetParent(parent.transform);
            StartCoroutine(_attackCommand.CooldownTimer());
            SetData(_weaponData.BaseHealth, shipStats.DodgeChanceMultiplier, Subsystem.Weapon, visuals);
            MaxHealth.UpdateBaseValue(_weaponData.BaseHealth);
        }
    }
}