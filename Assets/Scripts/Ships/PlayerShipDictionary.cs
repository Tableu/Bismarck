using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipDictionary : ScriptableObject
{
    private Dictionary<int, ShipData> shipDict;
    public int Count => shipDict.Count;
    
    public void AddShip(ShipData ship, int id)
    {
        shipDict.Add(id, ship);
    }

    public void RemoveShip(int id)
    {
        shipDict.Remove(id);
    }

    public void ClearDict()
    {
        shipDict.Clear();
    }

    public ShipData GetShip(int id)
    {
        return shipDict[id];
    }
}
