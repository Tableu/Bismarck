using Ships.DataManagement;

namespace Ships.Components
{
    /// <summary>
    /// An interface that is automatically called when a ship is spawned
    /// </summary>
    public interface IInitializableComponent
    {
        /// <summary>
        /// Automatically called by ship spawner on all ship components using this interface.
        /// </summary>
        /// <param name="data">The ShipData SO associated with this ship</param>
        /// <param name="spawner">The Spawner SO that created this shop</param>
        public void Initialize(ShipData data, ShipSpawner spawner);
    }
}