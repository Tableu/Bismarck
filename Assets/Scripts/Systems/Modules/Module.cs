using System;

namespace Systems.Modules
{
    [Serializable]
    public class Module
    {
        public ModuleData Data;
        public Coordinates RootPosition;

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
}