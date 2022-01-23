using Ships.DataManagment;

namespace Ships.Components
{
    public interface IInitializableComponent
    {
        public void Initialize(ShipData data, ShipSpawner spawner);
    }
}