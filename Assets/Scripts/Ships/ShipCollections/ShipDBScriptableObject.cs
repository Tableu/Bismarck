using System;
using System.Collections.Generic;
using Ships.DataManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipDB")]
[Serializable]
public class ShipDBScriptableObject : ScriptableObject
{
    [SerializeField] private List<ShipData> shipDB;

    public ShipData GetShip(string shipName)
    {
        return shipDB.Find(x => x.shipName == shipName);
    }
}