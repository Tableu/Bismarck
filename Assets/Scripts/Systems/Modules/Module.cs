using System;
using System.Collections.Generic;

namespace Systems.Modules
{
    [Serializable]
    public class Module
    {
        public ModuleData Data;
        public Coordinates PivotPosition;

        internal Module(ModuleData moduleData)
        {
            Data = moduleData;
        }
    }
    
    [Serializable]
    public struct Coordinates
    {
        public int x;
        public int y;
    }

    [Serializable]
    public class ModuleList
    {
        public List<Module> List;

        public static Module[,] ListsToGrid(List<ModuleList> moduleLists)
        {
            Module[,] Grid = new Module[moduleLists.Count, moduleLists[0].List.Count];
            for (var row = 0; row < moduleLists.Count; row++)
            {
                ModuleList moduleList = moduleLists[row];
                for (var column = 0; column < moduleList.List.Count; column++)
                {
                    Grid[row, column] = moduleList.List[column];
                }
            }

            return Grid;
        }

        public static List<ModuleList> GridToLists(Module[,] grid)
        {
            int columnLength = grid.GetLength(0);
            int rowHeight = grid.GetLength(1);
            List<ModuleList> List = new List<ModuleList>();
            for (var row = 0; row < grid.GetLength(1); row++)
            {
                ModuleList moduleList = new ModuleList();
                for (var column = 0; column < grid.GetLength(0); column++)
                {
                    moduleList.List.Add(grid[row,column]);
                }
                List.Add(moduleList);
            }

            return List;
        }
        
    }
}