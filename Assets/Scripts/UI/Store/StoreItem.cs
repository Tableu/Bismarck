using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private int cost;

    private void Start()
    {
        
    }
    public void SpawnShip()
    {
        if (StoreWindow.Instance.Buy(cost))
        {
            Instantiate(shipPrefab, Vector3.zero, Quaternion.identity, ShipManager.Instance.ShipParent.transform);
        }
    }
}
