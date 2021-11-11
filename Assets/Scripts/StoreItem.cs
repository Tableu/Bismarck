using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private int cost;

    public void SpawnShip()
    {
        if (StoreManager.Instance.Buy(cost))
        {
            Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
