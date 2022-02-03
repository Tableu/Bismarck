using System;
using System.Collections.Generic;
using Systems.Modifiers;
using UnityEngine;

namespace Systems.Modules
{
    [CreateAssetMenu(fileName = "Modules", menuName = "Modules/ShipData")]
    [Serializable]
    public class ModuleData : UniqueId
    {
        public ModifierData modifierData;
        public List<Coordinates> GridPositions;
        public Sprite GridSprite;

        public Module MakeModule()
        {
            Module module = new Module(this);
            return module;
        }
    }
}