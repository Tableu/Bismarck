using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipList")]
public class ShipListScriptableObject : ScriptableObject
{
    private List<GameObject> shipList;

    public int Count => shipList.Count;
    public List<GameObject> ShipList => shipList.ToList();
    public void AddShip(GameObject ship)
    {
        shipList.Add(ship);
    }

    public void RemoveShip(GameObject ship)
    {
        shipList.Remove(ship);
    }

    public GameObject GetShip(int id)
    {
        return shipList.Find(o => o.GetInstanceID() == id);
    }

    public void ClearList()
    {
        shipList.Clear();
    }

    public void OnEnable()
    {
        shipList = new List<GameObject>();
    }
}
