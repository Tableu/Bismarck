using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Ships.Components;
using Systems.Save;
using UnityEngine;

namespace Systems.Modules
{
    /// <summary>
    ///     Stores runtime information on Modules and Module Grid.
    ///     Saves and Loads Modules and Module Grid.
    ///     Will init with default values from ShipData if there is no save data.
    /// </summary>
    [Serializable]
    public class ModulesInfo : MonoBehaviour, ISavable
    {
        [SerializeField] private IdList _moduleIdList;
        private ModuleSlot[,] _slotGrid;
        private List<Module> _modules;
        private List<ModuleSlot> _moduleSlots;
        private int _rowHeight;
        private int _columnLength;

        private ShipInfo _info;

        public string id => "ModulesInfo";
        public int RowHeight => _rowHeight;
        public int ColumnLength => _columnLength;
        public List<Module> Modules => _modules;

        private void Start()
        {
            _info = GetComponent<ShipInfo>();
            if (_slotGrid == null)
            {
                _rowHeight = _info.Data.ModuleGridHeight;
                _columnLength = _info.Data.ModuleGridWidth;
                _slotGrid = new ModuleSlot[_rowHeight, _columnLength];
                _modules = _info.Data.ModuleList;
                _moduleSlots = _info.Data.ModuleSlots;
                foreach (Module module in _modules)
                {
                    AddModule(module);
                }

                foreach (ModuleSlot moduleSlot in _moduleSlots)
                {
                    AddModuleSlot(moduleSlot);
                }
            }
        }

        public Module GetModule(Vector2Int modulePos)
        {
            if (_slotGrid[modulePos.y, modulePos.x] != null)
            {
                return _slotGrid[modulePos.y, modulePos.x].module;
            }

            return null;
        }

        public bool AddModule(Module module)
        {
            if (ModulePositionValid(module))
            {
                Vector2Int rootPos = module.RootPosition;
                foreach (Vector2Int coords in module.Data.GridPositions)
                {
                    _slotGrid[rootPos.y + coords.y, rootPos.x + coords.x].module = module;
                }

                _modules.Add(module);
                return true;
            }

            return false;
        }

        public void RemoveModule(Vector2Int modulePos)
        {
            Module moduleToRemove = _slotGrid[modulePos.y, modulePos.x].module;
            Vector2Int rootPos = moduleToRemove.RootPosition;

            foreach (Vector2Int coords in moduleToRemove.Data.GridPositions)
            {
                Vector2Int pos = new Vector2Int(rootPos.x + coords.x, rootPos.y + coords.y);
                if (pos.x >= 0 && pos.x < _columnLength &&
                    pos.y >= 0 && pos.y < _rowHeight)
                {
                    if (_slotGrid[pos.y, pos.x].module == moduleToRemove)
                    {
                        _slotGrid[pos.y, pos.x].module = null;
                    }
                }
            }

            _modules.Remove(moduleToRemove);
        }

        public bool AddModuleSlot(ModuleSlot moduleSlot)
        {
            Vector2Int pos = moduleSlot.Position;
            if (pos.x >= 0 && pos.x < _columnLength &&
                pos.y >= 0 && pos.y < _rowHeight &&
                _slotGrid[pos.y, pos.x] == null)
            {
                _slotGrid[pos.y, pos.x] = moduleSlot;
                return true;
            }

            return false;
        }

        public bool ModulePositionValid(Module module)
        {
            Vector2Int rootPos = module.RootPosition;
            foreach (Vector2Int gridPos in module.Data.GridPositions)
            {
                Vector2Int pos = new Vector2Int(rootPos.x + gridPos.x, rootPos.y + gridPos.y);

                if (pos.x < 0 || pos.x >= _columnLength &&
                    pos.y < 0 || pos.y >= _rowHeight)
                {
                    Debug.Log("ModulesInfo: position out of bounds");
                    return false;
                }

                if (_slotGrid[pos.y, pos.x] == null ||
                    (_slotGrid[pos.y, pos.x].ValidTypes & module.Data.Type) == ModuleType.None ||
                    _slotGrid[pos.y, pos.x].module != null)
                {
                    Debug.Log("ModulesInfo: position invalid");
                    return false;
                }
            }

            return true;
        }

        public object SaveState()
        {
            return new SaveData
            {
                Modules = _modules,
                ModuleSlots = _moduleSlots
            };
        }

        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            _slotGrid = new ModuleSlot[_rowHeight, _columnLength];
            _modules = saveData.Modules;
            _moduleSlots = saveData.ModuleSlots;
            
            foreach (Module module in _modules)
            {
                module.Data = _moduleIdList.IDMap[module.ModuleId] as ModuleData;
                if (module.Data != null)
                {
                    AddModule(module);
                }
            }

            foreach (ModuleSlot moduleSlot in _moduleSlots)
            {
                AddModuleSlot(moduleSlot);
            }
        }

        [Serializable]
        private struct SaveData
        {
            public List<Module> Modules;
            public List<ModuleSlot> ModuleSlots;
        }
    }
}
