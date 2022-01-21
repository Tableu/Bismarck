using Systems.Modifiers;
using UnityEngine;

namespace Systems.Effects
{
    public abstract class IEffect : ScriptableObject
    {
        public abstract void Apply(ModifiableTarget target);
        public abstract void Remove(ModifiableTarget target);
    }
}