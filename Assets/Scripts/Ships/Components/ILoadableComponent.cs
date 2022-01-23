using Ships.DataManagement;

namespace Ships.Components
{
    public interface ILoadableComponent
    {
        public void Load(ShipSaveData saveData);
    }
}