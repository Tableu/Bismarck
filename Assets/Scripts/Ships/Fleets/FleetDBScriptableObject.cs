using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ships", menuName = "Ships/FleetDB")]
[Serializable]
public class FleetDBScriptableObject : ScriptableObject
{
    public List<RandomFleet> fleetDB;
}
