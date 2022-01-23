using System;
using System.Collections.Generic;
using System.IO;
using Ships.Components;
using Ships.DataManagement;
using StarMap;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Save
{
    /// <summary>
    ///     The class responsible for saving and loading the game.
    /// </summary>
    [CreateAssetMenu(fileName = "SaveManager", menuName = "SaveManager")]
    public class SaveManager : ScriptableObject
    {
        /// <summary>
        ///     A list containing all the ships to save
        /// </summary>
        [SerializeField] private ShipList shipsToSave;

        /// <summary>
        ///     The ship spawner to use to spawn the ships on loading
        /// </summary>
        [SerializeField] private ShipSpawner shipSpawner;

        /// <summary>
        ///     The list of UUIDs for the ship data scriptable objects
        /// </summary>
        [SerializeField] private UUIDList shipUuids;

        /// <summary>
        ///     The path to save and load to. Automatically set if not manually specified.
        /// </summary>
        [NonSerialized] public string savePath;

        public static string DefaultSavePath => Application.persistentDataPath + "/savedata.save";


        /// <summary>
        /// Saves the game to a json file. Can be called by right clicking the SaveManager object in the inspector.
        /// </summary>
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

        /// <summary>
        /// Tries to load a save game using the current savePath.
        /// </summary>
        /// <returns>True if loading succeeded, false otherwise</returns>
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

        /// <summary>
        /// A class that defines the structure of the game save file
        /// </summary>
        [Serializable]
        private class GameSaveData
        {
            public List<ShipSaveData> ships = new List<ShipSaveData>();
            public GameContext.GameState state;
        }
    }
}