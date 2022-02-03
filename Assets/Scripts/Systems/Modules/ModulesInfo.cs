using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Ships.Components;
using Systems.Save;
using UnityEngine;

namespace Systems.Modules
{
    [Serializable]
    public class ModulesInfo : MonoBehaviour, ISavable
    {
        public Module[,] Grid;
        public List<Module> Modules;
        public int RowHeight;
        public int ColumnLength;

        private ShipInfo _info;

        private void Awake()
        {
            _info = GetComponent<ShipInfo>();
            if (Grid == null)
            {
                Grid = ModuleList.ListsToGrid(_info.Data.ModuleGrid);
            }

            if (Modules == null)
            {
                Modules = new List<Module>();
            }
        }

        public void AddModule(Module module, int row, int column)
        {
            foreach(Coordinates coords in module.Data.GridPositions)
            {
                if (column + coords.x >= 0 && column + coords.x < ColumnLength &&
                    row + coords.y >= 0 && row + coords.y < RowHeight)
                {
                    Grid[row + coords.y, column + coords.x] = module;
                }
            }

            if (!Modules.Contains(module))
            {
                Modules.Add(module);
            }

            module.PivotPosition = new Coordinates
            {
                x = column,
                y = row
            };
        }

        public void RemoveModule(Module moduleToRemove)
        {
            foreach (Module module in Modules)
            {
                if (module == moduleToRemove)
                {
                    int row = module.PivotPosition.y;
                    int column = module.PivotPosition.x;
                    
                    foreach (Coordinates coords in module.Data.GridPositions)
                    {
                        if (column + coords.x > 0 && column + coords.x < ColumnLength &&
                            row + coords.y > 0 && row + coords.y < RowHeight)
                        {
                            Grid[row + coords.y, column + coords.x] = null;
                        }
                    }
                }
            }

            Modules.Remove(moduleToRemove);
        }
        public string id => "module_grid";
        public object SaveState()
        {
            return new SaveData
            {
                ModuleGrid = ModuleList.GridToLists(Grid),
                Modules = Modules
            };
        }

        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            Grid = ModuleList.ListsToGrid(saveData.ModuleGrid);
            Modules = saveData.Modules;
        }

        [Serializable]
        private struct SaveData
        {
            public List<ModuleList> ModuleGrid;
            public List<Module> Modules;
        }
    }
}
