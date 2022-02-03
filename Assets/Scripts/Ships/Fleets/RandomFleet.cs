using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/RandomFleet")]
[Serializable]
public class RandomFleet : ScriptableObject
{
    public List<RandomShipList> randomFleet;
    public GameObject fleetVisualsPrefab;
}
