using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipTurrets : MonoBehaviour
{
    [SerializeField] private List<Transform> turretPositions;

    [SerializeField] private Transform turretParent;

    private List<AttackScriptableObject> _weapons;

    public IReadOnlyCollection<Transform> TurretPositions => turretPositions;

    public void Refresh()
    {
        foreach (Transform child in turretParent)
        {
            Destroy(child.gameObject);
        }

        var len = Math.Min(turretPositions.Count, _weapons.Count);
        for (var i = 0; i < len; i++)
        {
            var turret = Instantiate(_weapons[i].Turret, turretParent, false);
            turret.transform.localPosition = turretPositions[i].localPosition;
            turret.layer = gameObject.layer;
        }
    }
}
