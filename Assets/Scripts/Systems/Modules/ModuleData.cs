using System;
using System.Collections.Generic;
using Systems.Modifiers;
using UnityEngine;

namespace Systems.Modules
{
    [Flags]
    [Serializable]
    public enum ModuleType
    {
        None = 0,
        Weapon = 1 << 0,
        Engine = 1 << 1
    }
    /// <summary>
    ///     Stores default values for a Module
    /// </summary>
    [CreateAssetMenu(fileName = "Modules", menuName = "Modules/ShipData")]
    [Serializable]
    public class ModuleData : UniqueId
    {
        public ModifierData modifierData;
        public List<Vector2Int> GridPositions;
        public Sprite GridSprite;
        public ModuleType Type;
        public string DisplayName;

        public Module MakeModule(Vector2Int rootPosition)
        {
            Module module = new Module()
            {
                RootPosition = rootPosition,
                Data = this
            };
            return module;
        }
    }
}