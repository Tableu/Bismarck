using System.Collections.Generic;
using Systems.Conditions;
using Systems.Effects;
using UnityEngine;

namespace Systems.Modifiers
{
    [CreateAssetMenu(fileName = "New Modifer", menuName = "Modifiers/Modifer", order = 0)]
    public class ModifierData : ScriptableObject
    {
        public List<EffectGroup> EffectGroups;
        public ConditionRule Condition;
        public float Duration;
        public bool HasDuration;

        public Modifer AttachNewModifer(IModifiableTarget target)
        {
            var modifer = new Modifer(this, target);
            target.AttachModifer(modifer);
            return modifer;
        }
    }
}