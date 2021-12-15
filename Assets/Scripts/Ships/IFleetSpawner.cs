using System.Collections.Generic;

public interface IFleetSpawner
{
    public void SpawnFleet(List<ShipData> shipDatas);
    public void SpawnShip(ShipData data);
}