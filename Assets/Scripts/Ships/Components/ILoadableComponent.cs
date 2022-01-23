using Ships.DataManagment;

namespace Ships.Components
{
    public interface ILoadableComponent
    {
        public void Load(ShipSaveData saveData);
    }
}