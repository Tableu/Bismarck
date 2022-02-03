using System;
using System.Collections.Generic;
using Ships.DataManagement;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RandomShipList
{
    [SerializeField] private List<ShipData> randomShips;
    [SerializeField] private List<int> shipWeights;

    public ShipData RandomShip()
    {
        List<int> intervals = new List<int>();
        int totalWeight = 0;
        foreach (int weight in shipWeights)
        {
            totalWeight += weight;
            intervals.Add(totalWeight);
        }

        float randomNumber = Random.Range(0, totalWeight);
        int index = 0;
        foreach (int interval in intervals)
        {
            if (randomNumber < interval)
            {
                return randomShips[index];
            }

            index++;
        }

        return randomShips[0];
    }
}
