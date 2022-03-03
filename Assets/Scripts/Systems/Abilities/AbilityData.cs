using System.Collections.Generic;
using Systems.Modifiers;
using UnityEngine;

namespace Systems.Abilities
{
    /// <summary>
    ///     Data for an ability that can be activated to apply modifiers to a target, with a set cooldown
    ///     between activations.
    /// </summary>
    public class AbilityData : ScriptableObject
    {
        [SerializeField] private List<ModifierData> modifiers;
        [SerializeField] private float cooldown;

        public List<ModifierData> Modifiers => modifiers;
        public float Cooldown => cooldown;
    }
}