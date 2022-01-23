using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Ships.Components;
using Ships.DataManagment;
using UnityEngine;

namespace Systems.Save
{
    [CreateAssetMenu(fileName = "SaveManager", menuName = "SaveManager")]
    public class SaveManager : ScriptableObject
    {
        [SerializeField] private ShipList shipsToSave;
        [SerializeField] private ShipSpawner shipSpawner;
        [SerializeField] private UUIDList shipUuids;

        [NonSerialized] public string savePath;

        public static SaveManager Instance { get; private set; }
        public static string DefaultSavePath => Application.persistentDataPath + "/savedata.save";

        private void Awake()
        {
            Debug.Assert(Instance == null, "Multiple save manager created!");
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        [ContextMenu("Save Game")]
        public void Save()
        {
            if (savePath is null || savePath == "") savePath = DefaultSavePath;
            var saveData = new GameSaveData();
            foreach (var ship in shipsToSave.Ships) saveData.ships.Add(new ShipSaveData(ship));
            saveData.state = GameContext.Instance.CurrentState;

            var saveString = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(savePath, saveString);
        }

        [ContextMenu("Load Game")]
        public bool Load()
        {
            if (savePath is null || savePath == "") savePath = DefaultSavePath;
            if (!File.Exists(savePath)) return false;

            // todo: catch exceptions
            var saveData = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(savePath));

            var parent = GameObject.FindWithTag("Ships") ?? new GameObject
            {
                name = "Ships",
                tag = "Ships"
            };
            foreach (var shipSaveData in saveData.ships)
            {
                var shipData = shipUuids.FindByUUID(shipSaveData.shipDataId) as ShipData;
                var ship = shipSpawner.SpawnShip(shipData, parent.transform, shipSaveData.position);
                if (ship is null)
                {
                    Debug.LogWarning("Failed to load a ship");
                    continue;
                    ;
                }

                foreach (var loadableComponent in ship.GetComponents<ILoadableComponent>())
                {
                    loadableComponent.Load(shipSaveData);
                }
            }

            return true;
        }

        [Serializable]
        private class GameSaveData
        {
            public List<ShipSaveData> ships = new List<ShipSaveData>();
            public GameContext.GameState state;
        }
    }
}