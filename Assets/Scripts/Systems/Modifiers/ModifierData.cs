using System.Collections.Generic;
using UnityEngine;

namespace Systems.Modifiers
{
    [CreateAssetMenu(fileName = "New Modifer", menuName = "Modifiers/Modifer", order = 0)]
    public class ModifierData : ScriptableObject
    {
        public string ModiferName;
        public List<IEffect> Effects;
        public ConditionRule Condition;
        public float Duration;
        public bool HasDuration;

        public Modifer AttachNewModifer(ModifiableTarget target)
        {
            var modifer = new Modifer(this, target);
            target.AttachModifer(modifer);
            return modifer;
        }
    }
}