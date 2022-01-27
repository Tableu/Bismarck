using System;
using Systems;
using Systems.Modifiers;
using UnityEngine;

namespace Modules
{
    [CreateAssetMenu(fileName = "Modules", menuName = "Modules/ShipData")]
    [Serializable]
    public class ModuleData : UuidScriptableObject
    {
        [SerializeField] public ModifierData modifierData;
        public Vector2 Pivot;
        public GameObject GridSprite;
    }
}