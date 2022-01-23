using Ships.DataManagement;

namespace Ships.Components
{
    /// <summary>
    /// An interface that is automatically called every time a ship is loaded from save data.
    /// </summary>
    public interface ILoadableComponent
    {
        /// <summary>
        /// Called automatically by the SaveManager whenever a ship is loaded from save data.
        /// </summary>
        /// <param name="saveData">The save data associated with this ship</param>
        public void Load(ShipSaveData saveData);
    }
}