using System;

namespace Systems.Modules
{
    /// <summary>
    ///     Holds data and root position of the module
    /// </summary>
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

    /// <summary>
    ///     int substitute for vectors
    /// </summary>
    [Serializable]
    public struct Coordinates
    {
        public int x;
        public int y;
    }
}