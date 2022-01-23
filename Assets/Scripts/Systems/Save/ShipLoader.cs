using UnityEngine;

namespace Systems.Save
{
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