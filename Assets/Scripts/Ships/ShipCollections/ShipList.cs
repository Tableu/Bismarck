using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/ShipList")]
public class ShipList : ScriptableObject
{
    private List<GameObject> shipList;

    public int Count => shipList.Count;
    public IReadOnlyList<GameObject> Ships => shipList;

    public void OnEnable()
    {
        shipList = new List<GameObject>();
    }

    public void AddShip(GameObject ship)
    {
        shipList.Add(ship);
        OnListChanged?.Invoke();
    }

    public void RemoveShip(GameObject ship)
    {
        shipList.Remove(ship);
        OnListChanged?.Invoke();
    }

    public GameObject GetShip(int id)
    {
        return shipList.Find(o => o.GetInstanceID() == id);
    }

    public void ClearList()
    {
        shipList.Clear();
        OnListChanged?.Invoke();
    }

    public event Action OnListChanged;
}