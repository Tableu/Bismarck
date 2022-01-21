using UnityEngine;

namespace Systems.Modifiers
{
    public abstract class IEffect : ScriptableObject
    {
        public abstract void Apply(ModifiableTarget target);
        public abstract void Remove(ModifiableTarget target);
    }
}