using System.Collections.Generic;
using UnityEngine;

public static class DetectionController
{
    private static readonly ContactFilter2D PlayerFilter = new ContactFilter2D
    {
        layerMask = LayerMask.GetMask("PlayerShips"),
        useLayerMask = true
    };
    private static readonly ContactFilter2D EnemyFilter = new ContactFilter2D
    {
        layerMask = LayerMask.GetMask("EnemyShips"),
        useLayerMask = true
    };

    public static GameObject DetectShip(float aggroRange, GameObject attacker)
    {
        List<Collider2D> results = new List<Collider2D>();
        if (attacker.layer.Equals(LayerMask.NameToLayer("PlayerShips")))
        {
            Physics2D.OverlapCircle(attacker.transform.position, aggroRange, EnemyFilter, results);
        }
        else if (attacker.layer.Equals(LayerMask.NameToLayer("EnemyShips")))
        {
            Physics2D.OverlapCircle(attacker.transform.position, aggroRange, PlayerFilter, results);
        }
        else
        {
            return null;
        }

        float currentMagnitude = aggroRange*aggroRange;
        GameObject target = null;
        foreach (Collider2D ship in results)
        {
            Vector2 diff = attacker.transform.position - ship.transform.position;
            if (diff.sqrMagnitude < currentMagnitude)
            {
                currentMagnitude = diff.sqrMagnitude;
                target = ship.transform.gameObject;
            }
        }
        return target;
    }
}
