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
        public string Id;
        public Vector2Int RootPosition;
    }
}