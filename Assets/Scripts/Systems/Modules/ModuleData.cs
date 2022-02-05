using System;
using System.Collections.Generic;
using Systems.Modifiers;
using UnityEngine;

namespace Systems.Modules
{
    /// <summary>
    ///     Stores default values for a Module
    /// </summary>
    [CreateAssetMenu(fileName = "Modules", menuName = "Modules/ShipData")]
    [Serializable]
    public class ModuleData : UniqueId
    {
        public ModifierData modifierData;
        public List<Coordinates> GridPositions;
        public Sprite GridSprite;
        public ModuleType Type;
        public string DisplayName;

        public Module MakeModule(Coordinates rootPosition)
        {
            Module module = new Module(this);
            module.RootPosition = rootPosition;
            return module;
        }
    }
}