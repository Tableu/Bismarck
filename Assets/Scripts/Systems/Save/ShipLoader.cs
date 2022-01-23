using UnityEngine;

namespace Systems.Save
{
    /// <summary>
    /// Simple class that automatically calls saveManager.Load() on Start.
    /// </summary>
    /// <remarks>
    /// Just a temporary solution until we create a method for saving & loading the whole game state.
    /// </remarks>
    public class ShipLoader : MonoBehaviour
    {
        [SerializeField] private SaveManager saveManager;

        private void Start()
        {
            // todo: find a better way of dealing with loading ships
            saveManager.Load();
        }
    }
}