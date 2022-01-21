using Systems.Modifiers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Effects
{
    public abstract class IEffect : ScriptableObject
    {
        public abstract void Apply(IModifiableTarget target);
        public abstract void Remove(IModifiableTarget target);
    }
}