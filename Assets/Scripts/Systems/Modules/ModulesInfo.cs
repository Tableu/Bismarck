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
        private List<Module> Modules;
        private int RowHeight;
        private int ColumnLength;

        private ShipInfo _info;

        private void Awake()
        {
            _info = GetComponent<ShipInfo>();
            if (Grid == null)
            {
                RowHeight = _info.Data.ModuleGridHeight;
                ColumnLength = _info.Data.ModuleGridWidth;
                Grid = new Module[RowHeight, ColumnLength];
                Modules = _info.Data.ModuleList;
                foreach (Module module in Modules)
                {
                    AddModule(module);
                }
            }
        }

        public Module GetModule(Coordinates modulePos)
        {
            return Grid[modulePos.y, modulePos.x];
        }

        public void AddModule(ModuleData moduleData, Coordinates modulePos)
        {
            Module module = moduleData.MakeModule(modulePos);
            foreach(Coordinates gridPos in moduleData.GridPositions)
            {
                if (modulePos.x + gridPos.x >= 0 && modulePos.x + gridPos.x < ColumnLength &&
                    modulePos.y + gridPos.y >= 0 && modulePos.y + gridPos.y < RowHeight)
                {
                    Grid[modulePos.y + gridPos.y, modulePos.x + gridPos.x] = module;
                }
            }
        }
        
        private void AddModule(Module module)
        {
            Coordinates rootPos = module.RootPosition;
            foreach(Coordinates coords in module.Data.GridPositions)
            {
                if (rootPos.x + coords.x >= 0 && rootPos.x + coords.x < ColumnLength &&
                    rootPos.y + coords.y >= 0 && rootPos.y + coords.y < RowHeight)
                {
                    Grid[rootPos.y + coords.y, rootPos.x + coords.x] = module;
                }
            }
            
            Modules.Add(module);
        }

        public void RemoveModule(Coordinates modulePos)
        {
            Module moduleToRemove = Grid[modulePos.y, modulePos.x];
            Coordinates rootPos = moduleToRemove.RootPosition;
            foreach (Coordinates coords in moduleToRemove.Data.GridPositions)
            {
                if (rootPos.x + coords.x > 0 && rootPos.x + coords.x < ColumnLength &&
                    rootPos.y + coords.y > 0 && rootPos.y + coords.y < RowHeight)
                {
                    Grid[rootPos.y + coords.y, rootPos.x + coords.x] = null;
                }
            }
            
            Modules.Remove(moduleToRemove);
        }
        public string id => "module_grid";
        public object SaveState()
        {
            return new SaveData
            {
                GridHeight = RowHeight,
                GridWidth = ColumnLength,
                Modules = Modules
            };
        }

        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            RowHeight = saveData.GridHeight;
            ColumnLength = saveData.GridWidth;
            Grid = new Module[RowHeight, ColumnLength];
            Modules = saveData.Modules;
            foreach (Module module in Modules)
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
