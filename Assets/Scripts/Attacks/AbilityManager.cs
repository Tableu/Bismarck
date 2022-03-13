using System.Collections.Generic;
using Ships.Components;
using Ships.DataManagement;
using Systems.Abilities;
using UnityEngine;
using Weapons;

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
        Debug.Assert(_data == null, "ShipInfo.data overwritten");
        _shipStats = shipStats;
        _data = shipData;
        InitializeWeapons();
        InitializeAbilities();
    }

    [ContextMenu("InitializeWeapons")]
    public void InitializeWeapons()
    {
        foreach (WeaponData weaponData in _data.Weapons)
        {
            GameObject turret = Instantiate(weaponData.Turret, _shipStats.Visuals.transform);
            turret.transform.position = _shipStats.Visuals.transform.position;
            Weapon weapon = gameObject.AddComponent<Weapon>();
            weapon.Initialize(_shipStats, weaponData);
            _weapons.Add(weapon);
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

    public void SetWeaponsTarget(DamageableComponent target)
    {
        if (target != null)
        {
            foreach (Weapon weapon in _weapons)
            {
                weapon.Attack.SetTarget(target);
            }
        }
    }

#if UNITY_EDITOR
    [Header("Test")] public DamageableComponent TestTarget;
    [ContextMenu("Set Weapon Target")]
    private void TestSetWeaponsTarget()
    {
        SetWeaponsTarget(TestTarget);
    }
#endif
}