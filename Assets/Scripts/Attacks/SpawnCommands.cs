using System.Collections;
using UnityEngine;

public interface SpawnUnitCommand
{
    public IEnumerator DoSpawnUnit(GameObject mothership);
    public void StopSpawnUnit();
    public void SetTarget(GameObject target);
}

public abstract class SpawnUnitScriptableObject : ScriptableObject
{
    public abstract SpawnUnitCommand MakeSpawnUnit(ShipSpawner shipSpawner);
}