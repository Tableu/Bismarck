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
        [NonSerialized] public ModuleData Data;
        public string ModuleId;
        public Vector2Int RootPosition;
        
        internal Module(ModuleData moduleData)
        {
            Data = moduleData;
            ModuleId = moduleData.id;
        }
    }
}