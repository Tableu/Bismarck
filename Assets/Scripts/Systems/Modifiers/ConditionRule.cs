using UnityEngine;

namespace Systems.Modifiers
{
    public abstract class ConditionRule : ScriptableObject
    {
        public abstract ICondition NewBinding(ModifiableTarget target);
    }
}