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
        private List<Module> _modules;
        private int _rowHeight;
        private int _columnLength;

        private ShipInfo _info;

        public string id => "module_grid";
        public int RowHeight => _rowHeight;
        public int ColumnLength => _columnLength;

        private void Awake()
        {
            _info = GetComponent<ShipInfo>();
            if (_grid == null)
            {
                _rowHeight = _info.Data.ModuleGridHeight;
                _columnLength = _info.Data.ModuleGridWidth;
                _grid = new Module[_rowHeight, _columnLength];
                _modules = _info.Data.ModuleList;
                foreach (Module module in _modules)
                {
                    AddModule(module);
                }
            }
        }

        public Module GetModule(Coordinates modulePos)
        {
            return _grid[modulePos.y, modulePos.x];
        }

        public bool AddModule(ModuleData moduleData, Coordinates modulePos)
        {
            foreach (Coordinates gridPos in moduleData.GridPositions)
            {
                if (modulePos.x + gridPos.x >= 0 && modulePos.x + gridPos.x < _columnLength &&
                    modulePos.y + gridPos.y >= 0 && modulePos.y + gridPos.y < _rowHeight)
                {
                    if (_grid[modulePos.y + gridPos.y, modulePos.x + gridPos.x] != null)
                    {
                        return false;
                    }
                }
            }
            
            Module module = moduleData.MakeModule(modulePos);
            foreach(Coordinates gridPos in moduleData.GridPositions)
            {
                if (modulePos.x + gridPos.x >= 0 && modulePos.x + gridPos.x < _columnLength &&
                    modulePos.y + gridPos.y >= 0 && modulePos.y + gridPos.y < _rowHeight)
                {
                    _grid[modulePos.y + gridPos.y, modulePos.x + gridPos.x] = module;
                }
            }

            _modules.Add(module);
            return true;
        }

        private bool AddModule(Module module)
        {
            Coordinates rootPos = module.RootPosition;

            foreach (Coordinates gridPos in module.Data.GridPositions)
            {
                if (rootPos.x + gridPos.x >= 0 && rootPos.x + gridPos.x < _columnLength &&
                    rootPos.y + gridPos.y >= 0 && rootPos.y + gridPos.y < _rowHeight)
                {
                    if (_grid[rootPos.y + gridPos.y, rootPos.x + gridPos.x] != null)
                    {
                        return false;
                    }
                }
            }
            
            foreach(Coordinates coords in module.Data.GridPositions)
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

        public void RemoveModule(Coordinates modulePos)
        {
            Module moduleToRemove = _grid[modulePos.y, modulePos.x];
            Coordinates rootPos = moduleToRemove.RootPosition;
            foreach (Coordinates coords in moduleToRemove.Data.GridPositions)
            {
                if (rootPos.x + coords.x > 0 && rootPos.x + coords.x < _columnLength &&
                    rootPos.y + coords.y > 0 && rootPos.y + coords.y < _rowHeight)
                {
                    _grid[rootPos.y + coords.y, rootPos.x + coords.x] = null;
                }
            }

            _modules.Remove(moduleToRemove);
        }
        public object SaveState()
        {
            return new SaveData
            {
                GridHeight = _rowHeight,
                GridWidth = _columnLength,
                Modules = _modules
            };
        }

        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            _rowHeight = saveData.GridHeight;
            _columnLength = saveData.GridWidth;
            _grid = new Module[_rowHeight, _columnLength];
            _modules = saveData.Modules;
            foreach (Module module in _modules)
            {
                AddModule(module);
            }
        }

        [Serializable]
        private struct SaveData
        {
            public int GridHeight;
            public int GridWidth;
            public List<Module> Modules;
        }
    }
}
