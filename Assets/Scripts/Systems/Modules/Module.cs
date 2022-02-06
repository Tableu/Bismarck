using System;
using UnityEngine;

namespace Systems.Modules
{
    /// <summary>
    ///     Holds data and root position of the module
    /// </summary>
    [Serializable]
    public class Module
    {
        public ModuleData Data;
        public Vector2Int RootPosition;

        internal Module(ModuleData moduleData)
        {
            Data = moduleData;
        }
    }
}