using System;

namespace Systems.Modifiers
{
    public interface ICondition
    {
        public bool IsTrue { get; }
        public event Action<bool> OnChange;
    }
}