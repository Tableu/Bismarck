using System;

namespace Systems.Modules
{
    /// <summary>
    ///     Represents different module types
    /// </summary>
    [Flags]
    [Serializable]
    public enum ModuleType
    {
        None,
        Weapon,
        Engine
    }

    /// <summary>
    ///     Stores the valid module types for a specific position on the module grid
    /// </summary>
    [Serializable]
    public class ModuleSlot
    {
        public Coordinates Position;
        public ModuleType ValidTypes;
    }
}