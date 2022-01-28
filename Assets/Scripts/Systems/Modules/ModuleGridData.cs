using System.Collections;
using System.Collections.Generic;
using Modules;
using Systems;
using UnityEngine;

namespace Modules
{
    public class ModuleGridData : UuidScriptableObject
    {
        public ModuleData[,] Grid;
        public List<ModuleData> ModuleDatas;
        public int RowHeight;
        public int ColumnLength;

        public void AddModule(ModuleData moduleData, int row, int column)
        {
            foreach(Coordinates coords in moduleData.GridPositions)
            {
                if (column + coords.x > 0 && column + coords.x < ColumnLength &&
                    row + coords.y > 0 && row + coords.y < RowHeight)
                {
                    Grid[row + coords.y, column + coords.x] = moduleData;
                }
            }
            ModuleDatas.Add(moduleData);

            moduleData.PivotPosition = new Coordinates
            {
                x = column,
                y = row
            };
        }

        public void RemoveModule(ModuleData moduleToRemove)
        {
            foreach (ModuleData moduleData in ModuleDatas)
            {
                if (moduleData == moduleToRemove)
                {
                    int row = moduleData.PivotPosition.y;
                    int column = moduleData.PivotPosition.x;
                    
                    foreach (Coordinates coords in moduleData.GridPositions)
                    {
                        if (column + coords.x > 0 && column + coords.x < ColumnLength &&
                            row + coords.y > 0 && row + coords.y < RowHeight)
                        {
                            Grid[row + coords.y, column + coords.x] = null;
                        }
                    }
                }
            }

            ModuleDatas.Remove(moduleToRemove);
        }
    }
}
