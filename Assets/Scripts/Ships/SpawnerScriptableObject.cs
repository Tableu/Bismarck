using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerScriptableObject : ScriptableObject
{
    public abstract void SpawnFleet(List<ShipData> shipDatas, Transform parent);
    public abstract GameObject SpawnShip(ShipData data, Transform parent, Vector3? startPos = null);
}