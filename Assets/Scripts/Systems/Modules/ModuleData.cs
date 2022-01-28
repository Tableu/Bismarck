using System;
using System.Collections.Generic;
using Systems;
using Systems.Modifiers;
using UnityEngine;

namespace Modules
{
    [CreateAssetMenu(fileName = "Modules", menuName = "Modules/ShipData")]
    [Serializable]
    public class ModuleData : UuidScriptableObject
    {
        public ModifierData modifierData;
        public List<Coordinates> GridPositions;
        public Coordinates PivotPosition;
        public GameObject GridSprite;
    }

    [Serializable]
    public struct Coordinates
    {
        public int x;
        public int y;
    }
}