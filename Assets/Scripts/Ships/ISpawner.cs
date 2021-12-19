using System.Collections.Generic;
using UnityEngine;


public interface ISpawner
{
    public void SpawnFleet(List<ShipData> shipDatas);
    public void SpawnShip(ShipData data);
}

public abstract class SpawnerScriptableObject : ScriptableObject
{
    public abstract void SpawnFleet(List<ShipData> shipDatas);
    public abstract void SpawnShip(ShipData data);
}