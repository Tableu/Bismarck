using Systems.Modifiers;
using UnityEngine;

namespace Systems.Conditions
{
    public abstract class ConditionRule : ScriptableObject
    {
        public abstract ICondition NewBinding(IModifiableTarget target);
    }
}