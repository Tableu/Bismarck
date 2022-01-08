using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)][Serializable]
public class ShipSpawner : SpawnerScriptableObject
{
    public ShipDictionary ShipDictionary;
    public ShipListScriptableObject ShipList;
    public LayerMask LayerMask;
    public string ShipLayer;
    public int StartDirection;
    public Transform ProjectileParent;

    public override void SpawnFleet(List<ShipData> shipDatas, Transform parent)
    {
        foreach (ShipData ship in shipDatas)
        {
            SpawnShip(ship, parent);
        }
    }

    public override GameObject SpawnShip(ShipData data, Transform parent, Vector3? startPos = null)
    {
        if (parent == null)
            return null;
        GameObject ship = Instantiate(data.ShipPrefab, parent, false);
        if (ship != null)
        {
            if (startPos != null)
            {
                ship.transform.position = startPos.Value;
                data.StartingPos = ship.transform.localPosition;
            }
            else
            {
                ship.transform.localPosition = data.StartingPos;
            }

            ShipLogic shipLogic = ship.GetComponent<ShipLogic>();
            if (shipLogic != null)
            {
                shipLogic.ShipSpawner = this;
            }
            
            ShipTurrets shipTurrets = ship.GetComponent<ShipTurrets>();
            if (shipTurrets != null)
            {
                shipTurrets.ShipSpawner = this;
            }
            
            ShipHealth shipHealth = ship.GetComponent<ShipHealth>();
            if (shipHealth != null)
            {
                shipHealth.shipDict = ShipDictionary;
            }

            var scale = ship.transform.localScale;
            ship.transform.localScale = new Vector3(scale.x*Math.Sign(StartDirection), scale.y, scale.z);
            ship.layer = LayerMask.NameToLayer(ShipLayer);
            
            ShipDictionary.AddShip(data,ship.GetInstanceID());
            ShipList.AddShip(ship);
            return ship;
        }
        return null;
    }
}
