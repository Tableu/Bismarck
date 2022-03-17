using System;
using Ships.Components;
using Ships.DataManagement;
using Ships.Fleets;
using Systems.Abilities;
using UnityEngine;

public class TargetingHelper : MonoBehaviour
{
    private ShipStats _shipStats;
    private ShipData _data;
    private DamageableComponent _target;
    public DamageableComponent Target => _target;

    public void Initialize(ShipData shipData, ShipStats shipStats)
    {
        // This function is not expected to be called multiple times or if ship data is provided via the inspector
        Debug.Assert(_data == null, "ShipInfo.data overwritten");
        _shipStats = shipStats;
        _data = shipData;
    }

    public void SetTarget(DamageableComponent target)
    {
        _target = target;
        OnTargetChanged?.Invoke();
    }

    public bool IsEnemy(ShipStats target)
    {
        _shipStats.Fleet.AgroStatusMap.TryGetValue(target.Fleet, out FleetAgroStatus fleetAgroStatus);
        return fleetAgroStatus == FleetAgroStatus.Hostile || fleetAgroStatus == FleetAgroStatus.Neutral;
    }

    public bool InRange(Ability ability)
    {
        if (_target != null &&
            (_shipStats.transform.position - _target.transform.position).magnitude < ability.MaxRange)
        {
            return true;
        }

        return false;
    }

    public bool TargetingSelf(Ability ability)
    {
        if (_target == null && ability.Data.ValidTargets.HasFlag(FleetAgroStatus.Self))
        {
            return true;
        }

        return false;
    }

    public event Action OnTargetChanged;
}