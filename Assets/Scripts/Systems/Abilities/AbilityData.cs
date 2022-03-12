using System.Collections.Generic;
using Systems.Modifiers;
using UI.InfoWindow;
using UnityEngine;

namespace Systems.Abilities
{
    /// <summary>
    ///     Data for an ability that can be activated to apply modifiers to a target, with a set cooldown
    ///     between activations.
    /// </summary>
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability", order = 0)]
    public class AbilityData : UniqueId
    {
        [SerializeField] private List<ModifierData> modifiers;
        [SerializeField] private float cooldown;
        [SerializeField] private ButtonData buttonData;

        public List<ModifierData> Modifiers => modifiers;
        public float Cooldown => cooldown;
        public ButtonData ButtonData => buttonData;
    }
}