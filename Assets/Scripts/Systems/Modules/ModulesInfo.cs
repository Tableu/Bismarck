using System;
using System.Collections.Generic;
using System.Linq;
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
        public event EventHandler ModulesInfoInitialized;
        [SerializeField] private IdList _moduleIdList;
        [SerializeField] private SaveManager _saveManager;
        private Module[,] _grid;
        private List<Module> _modules;
        private int _rowHeight;
        private int _columnLength;

        private ShipInfo _info;

        public string id => "ModulesInfo";
        public int RowHeight => _rowHeight;
        public int ColumnLength => _columnLength;
        public List<Module> Modules => _modules;

        protected virtual void OnInitialized(EventArgs e)
        {
            EventHandler handler = ModulesInfoInitialized;
            handler?.Invoke(this, e);
        }
        
        private void Start()
        {
            _info = GetComponent<ShipInfo>();
            if (_grid == null)
            {
                _rowHeight = _info.Data.ModuleGridHeight;
                _columnLength = _info.Data.ModuleGridWidth;
                _grid = new Module[_rowHeight, _columnLength];
                _modules = new List<Module>();

                foreach (Module module in _info.Data.ModuleList.ToList())
                {
                    if (module != null)
                    {
                        Module copy = new Module(_moduleIdList.IDMap[module.Id] as ModuleData);
                        copy.RootPosition = module.RootPosition;
                        AddModule(copy);
                    }
                }

                OnInitialized(EventArgs.Empty);
            }
        }

        public Module GetModule(Vector2Int modulePos)
        {
            if (_grid[modulePos.y, modulePos.x] != null)
            {
                return _grid[modulePos.y, modulePos.x];
            }

            return null;
        }

        public bool AddModule(Module module, Vector2Int newPos)
        {
            Vector2Int oldPos = module.RootPosition;
            module.RootPosition = newPos;
            if (ModulePositionValid(module))
            {
                foreach (Vector2Int coords in module.Data.GridPositions)
                {
                    Vector2Int pos = new Vector2Int(oldPos.x + coords.x, oldPos.y + coords.y);
                    if (pos.x >= 0 && pos.x < _columnLength &&
                        pos.y >= 0 && pos.y < _rowHeight)
                    {
                        if (_grid[pos.y, pos.x] == module)
                        {
                            _grid[pos.y, pos.x] = null;
                        }
                    }
                }
                
                foreach (Vector2Int coords in module.Data.GridPositions)
                {
                    _grid[newPos.y + coords.y, newPos.x + coords.x] = module;
                }

                Modules.Add(module);
                return true;
            }

            module.RootPosition = oldPos;
            return false;
        }

        //For init
        private void AddModule(Module module)
        {
            foreach (Vector2Int coords in module.Data.GridPositions)
            {
                _grid[module.RootPosition.y + coords.y, module.RootPosition.x + coords.x] = module;
            }

            Modules.Add(module);
        }

        public bool RemoveModule(Vector2Int modulePos)
        {
            Module moduleToRemove = _grid[modulePos.y, modulePos.x];
            if (moduleToRemove == null)
            {
                return false;
            }
            Vector2Int rootPos = moduleToRemove.RootPosition;

            foreach (Vector2Int coords in moduleToRemove.Data.GridPositions)
            {
                Vector2Int pos = new Vector2Int(rootPos.x + coords.x, rootPos.y + coords.y);
                if (pos.x >= 0 && pos.x < _columnLength &&
                    pos.y >= 0 && pos.y < _rowHeight)
                {
                    if (_grid[pos.y, pos.x] == moduleToRemove)
                    {
                        _grid[pos.y, pos.x] = null;
                    }
                }
            }

            Modules.Remove(moduleToRemove);
            return true;
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

                if (_grid[pos.y, pos.x] != null)
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
                Modules = _modules
            };
        }

        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            _grid = new Module[_rowHeight, _columnLength];
            _modules = saveData.Modules;

            foreach (Module module in _modules)
            {
                if (module != null)
                {
                    module.Data = _moduleIdList.IDMap[module.Id] as ModuleData;
                }
            }

            OnInitialized(EventArgs.Empty);
        }

        [Serializable]
        private struct SaveData
        {
            public List<Module> Modules;
        }
    }
}
