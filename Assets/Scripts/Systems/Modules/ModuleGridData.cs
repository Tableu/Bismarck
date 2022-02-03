using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems.Modules
{
    [Serializable]
    public class ModuleGridData
    {
        public Module[,] Grid;
        public List<Module> Modules;
        public int RowHeight;
        public int ColumnLength;

        public void Initialize()
        {
            if (Modules != null && RowHeight > 0 && ColumnLength > 0)
            {
                Grid = new Module[RowHeight, ColumnLength];
                foreach (Module module in Modules.ToList())
                {
                    var pivot = module.PivotPosition;
                    AddModule(module, pivot.y, pivot.x);
                }
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
    }
}
