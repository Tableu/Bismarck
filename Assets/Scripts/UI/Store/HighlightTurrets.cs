using System.Collections.Generic;
using UnityEngine;

public class HighlightTurrets : MonoBehaviour
{
    public ShipList playerShips;
    private List<SpriteRenderer> _highlightedTurrets = new List<SpriteRenderer>();

    public void Highlight()
    {
        if (playerShips == null)
        {
            return;
        }

        foreach (GameObject ship in playerShips.Ships)
        {
            if (ship == null)
            {
                continue;
            }

            Transform turrets = ship.transform.Find("Turrets");
            foreach (SpriteRenderer turret in turrets.GetComponentsInChildren<SpriteRenderer>())
            {
                if (turret != null)
                {
                    turret.color = Color.green;
                    _highlightedTurrets.Add(turret);
                }
            }
        }
    }

    public void DeHighlight()
    {
        if (_highlightedTurrets == null)
        {
            return;
        }

        foreach (SpriteRenderer turret in _highlightedTurrets)
        {
            if (turret != null)
            {
                turret.color = Color.white;
            }
        }

        _highlightedTurrets.Clear();
    }
}