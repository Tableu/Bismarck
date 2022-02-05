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
        private Module[,] _grid;
        private ModuleSlot[,] _slotGrid;
        private List<Module> _modules;
        private List<ModuleSlot> _moduleSlots;
        private int _rowHeight;
        private int _columnLength;

        private ShipInfo _info;

        public string id => "module_grid";
        public int RowHeight => _rowHeight;
        public int ColumnLength => _columnLength;
        public List<Module> Modules => _modules;

        private void Awake()
        {
            _info = GetComponent<ShipInfo>();
            if (_grid == null)
            {
                _rowHeight = _info.Data.ModuleGridHeight;
                _columnLength = _info.Data.ModuleGridWidth;
                _grid = new Module[_rowHeight, _columnLength];
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

        public Module GetModule(Coordinates modulePos)
        {
            return _grid[modulePos.y, modulePos.x];
        }

        public bool AddModule(Module module)
        {
            if (ModulePositionValid(module))
            {
                Coordinates rootPos = module.RootPosition;
                foreach (Coordinates coords in module.Data.GridPositions)
                {
                    if (rootPos.x + coords.x >= 0 && rootPos.x + coords.x < _columnLength &&
                        rootPos.y + coords.y >= 0 && rootPos.y + coords.y < _rowHeight)
                    {
                        _grid[rootPos.y + coords.y, rootPos.x + coords.x] = module;
                    }
                }

                _modules.Add(module);
                return true;
            }

            return false;
        }

        public void RemoveModule(Coordinates modulePos)
        {
            Module moduleToRemove = _grid[modulePos.y, modulePos.x];
            Coordinates rootPos = moduleToRemove.RootPosition;
            
            foreach (Coordinates coords in moduleToRemove.Data.GridPositions)
            {
                if (rootPos.x + coords.x > 0 && rootPos.x + coords.x < _columnLength &&
                    rootPos.y + coords.y > 0 && rootPos.y + coords.y < _rowHeight)
                {
                    if (_grid[rootPos.y + coords.y, rootPos.x + coords.x] == moduleToRemove)
                    {
                        _grid[rootPos.y + coords.y, rootPos.x + coords.x] = null;
                    }
                }
            }

            _modules.Remove(moduleToRemove);
        }

        public bool AddModuleSlot(ModuleSlot moduleSlot)
        {
            Coordinates pos = moduleSlot.Position;
            if (pos.x > 0 && pos.x < _columnLength &&
                pos.y > 0 && pos.y < _rowHeight &&
                _slotGrid[pos.y, pos.x] == null)
            {
                _slotGrid[pos.y, pos.x] = moduleSlot;
                return true;
            }

            return false;
        }

        public bool ModulePositionValid(Module module)
        {
            Coordinates rootPos = module.RootPosition;
            foreach (Coordinates gridPos in module.Data.GridPositions)
            {
                if (rootPos.x + gridPos.x >= 0 && rootPos.x + gridPos.x < _columnLength &&
                    rootPos.y + gridPos.y >= 0 && rootPos.y + gridPos.y < _rowHeight)
                {
                    if (_grid[rootPos.y + gridPos.y, rootPos.x + gridPos.x] != null ||
                        _slotGrid[rootPos.y + gridPos.y, rootPos.x + gridPos.x] == null ||
                        (_slotGrid[rootPos.y + gridPos.y, rootPos.x + gridPos.x].ValidTypes & module.Data.Type) ==
                        ModuleType.None)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public object SaveState()
        {
            return new SaveData
            {
                GridHeight = _rowHeight,
                GridWidth = _columnLength,
                Modules = _modules,
                ModuleSlots = _moduleSlots
            };
        }

        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            _rowHeight = saveData.GridHeight;
            _columnLength = saveData.GridWidth;
            _grid = new Module[_rowHeight, _columnLength];
            _modules = saveData.Modules;
            _moduleSlots = saveData.ModuleSlots;
            
            foreach (Module module in _modules)
            {
                AddModule(module);
            }

            foreach (ModuleSlot moduleSlot in _moduleSlots)
            {
                AddModuleSlot(moduleSlot);
            }
        }

        [Serializable]
        private struct SaveData
        {
            public int GridHeight;
            public int GridWidth;
            public List<Module> Modules;
            public List<ModuleSlot> ModuleSlots;
        }
    }
}
