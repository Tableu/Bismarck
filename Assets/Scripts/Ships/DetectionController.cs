using UnityEngine;

public static class DetectionController
{
    public static GameObject DetectShip(float aggroRange, GameObject attacker)
    {
        float currentMagnitude = aggroRange;
        GameObject target = null;
        foreach (GameObject ship in ShipManager.Instance.EnemyShips(attacker))
        {
            if (ship == null)
                continue;
            Vector2 diff = attacker.transform.position - ship.transform.position;
            if (diff.magnitude < aggroRange && diff.magnitude < currentMagnitude)
            {
                currentMagnitude = diff.magnitude;
                target = ship;
            }
        }
        return target;
    }
}
