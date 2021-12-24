using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawners", menuName = "Spawners/ShipSpawner", order = 0)][Serializable]
public class ShipSpawner : SpawnerScriptableObject
{
    public ShipDictionary ShipDictionary;
    public ShipListScriptableObject ShipList;
    public string ShipLayer;

    public override void SpawnFleet(List<ShipData> shipDatas, Transform parent)
    {
        foreach (ShipData ship in shipDatas)
        {
            SpawnShip(ship, parent);
        }
    }

    public override void SpawnShip(ShipData data, Transform parent, Vector3? startPos = null)
    {
        if (parent == null)
            return;
        GameObject ship = Instantiate(data.ShipPrefab, parent, false);
        if (ship != null)
        {
            if (startPos != null)
            {
                ship.transform.localPosition = startPos.Value;
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
            ShipHealth shipHealth = ship.GetComponent<ShipHealth>();
            if (shipHealth != null)
            {
                shipHealth.shipDict = ShipDictionary;
            }

            var scale = ship.transform.localScale;
            ship.transform.localScale = new Vector3(scale.x*data.ShipDirection, scale.y, scale.z);
            ship.layer = LayerMask.NameToLayer(ShipLayer);
            
            ShipDictionary.AddShip(data.Copy(),ship.GetInstanceID());
            ShipList.AddShip(ship);
        }
    }
}
