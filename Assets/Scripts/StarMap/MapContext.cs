using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarMap
{
    [CreateAssetMenu(fileName = "Context", menuName = "Map/Context", order = 0)]
    public class MapContext : ScriptableObject
    {
        public StarSystem StartingSystem;
        public MapData Data;
        public StarSystem CurrentSystem { get; private set; }

        private void Awake()
        {
            CurrentSystem = StartingSystem;
        }

        public event Action<StarSystem, StarSystem> OnCurrentSystemChange;

        public bool IsAccessible(StarSystem system)
        {
            return Data.SystemPairs.Any(systemPair =>
                (systemPair.System1 == system || systemPair.System2 == system) &&
                (systemPair.System1 == CurrentSystem || systemPair.System2 == CurrentSystem));
        }

        public void LoadSystem(StarSystem system)
        {
            // Check if is accessible from current system
            if (!IsAccessible(system))
            {
                Debug.Assert(IsAccessible(system),
                    $"Trying to jump to inaccessible system {system.SystemName} from {CurrentSystem.SystemName}");
                return;
            }

            // todo: update/save state of current system
            SceneManager.LoadScene("Scenes/BattleScene");
            OnCurrentSystemChange?.Invoke(CurrentSystem, system);
            CurrentSystem = system;
            CurrentSystem.LoadSystem();
        }
    }
}