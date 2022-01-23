using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipDictionary")]
public class ShipDictionary : ScriptableObject
{
    private Dictionary<int, ShipData> shipDict;
    public int Count => shipDict.Count;

    private void OnEnable()
    {
        shipDict = new Dictionary<int, ShipData>();
    }

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

    public void UpdateShip(int id, ShipData shipData)
    {
        shipDict[id] = shipData;
    }

    public Dictionary<int, ShipData>.Enumerator GetEnumerator()
    {
        return shipDict.GetEnumerator();
    }

    public List<ShipData> ShipDataList()
    {
        return shipDict.Values.ToList();
    }
}