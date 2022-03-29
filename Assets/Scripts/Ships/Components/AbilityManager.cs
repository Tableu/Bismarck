using System.Collections.Generic;
using Ships.DataManagement;
using Systems.Abilities;
using UnityEngine;
using Weapons;

namespace Ships.Components
{
    /// <summary>
    ///     Initializes and stores references to weapons and abilities
    /// </summary>
    public class AbilityManager : MonoBehaviour
    {
        private ShipStats _shipStats;
        private List<Weapon> _weapons = new List<Weapon>();
        private List<Ability> _abilities = new List<Ability>();
        private ShipData _data;
        public ShipData Data => _data;
        public List<Ability> Abilities => _abilities;
        public List<Weapon> Weapons => _weapons;

        public void Initialize(ShipData shipData, ShipStats shipStats)
        {
            // This function is not expected to be called multiple times or if ship data is provided via the inspector
            Debug.Assert(_data == null, "AbilityManager.data overwritten");
            _shipStats = shipStats;
            _data = shipData;
            InitializeWeapons();
            InitializeAbilities();
        }

        [ContextMenu("InitializeWeapons")]
        public void InitializeWeapons()
        {
            ShipTurrets turrets = _shipStats.Visuals.GetComponent<ShipTurrets>();
            using IEnumerator<Transform> enumerator = turrets.TurretPositions.GetEnumerator();
            
            foreach (WeaponData weaponData in _data.Weapons)
            {
                enumerator.MoveNext();
                GameObject turret = Instantiate(weaponData.Turret, _shipStats.Visuals.transform);
                turret.transform.position = enumerator.Current != null
                    ? enumerator.Current.position
                    : _shipStats.Visuals.transform.position;
                
                Weapon weapon = gameObject.AddComponent<Weapon>();
                weapon.Initialize(_shipStats, weaponData, turret);
                _weapons.Add(weapon);
                _abilities.Add(weapon.Attack);
            }
        }

        [ContextMenu("InitializeAbilities")]
        public void InitializeAbilities()
        {
            foreach (AbilityData abilityData in _data.Abilities)
            {
                Ability ability = new Ability(abilityData, _shipStats);
                _abilities.Add(ability);
            }
        }
    }
}