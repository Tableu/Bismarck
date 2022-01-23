using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipDB")]
[Serializable]
public class ShipDBScriptableObject : ScriptableObject
{
    [SerializeField] private List<ShipDataScriptableObject> shipDB;

    public ShipDataScriptableObject GetShip(string shipName)
    {
        return shipDB.Find(x => x.ShipData.ShipName == shipName);
    }
}