using System;
using UnityEngine;

namespace Systems.Modules
{
    /// <summary>
    ///     Represents different module types
    /// </summary>
    [Flags]
    [Serializable]
    public enum ModuleType
    {
        None = 0,
        Weapon = 1 << 0,
        Engine = 1 << 1
    }

    /// <summary>
    ///     Stores the valid module types for a specific position on the module grid
    /// </summary>
    [Serializable]
    public class ModuleSlot
    {
        public Vector2Int Position;
        public Module module;
        public ModuleType ValidTypes;
    }
}