using System;

namespace Systems.Conditions
{
    public interface ICondition
    {
        public bool IsTrue { get; }
        public event Action<bool> OnChange;
    }
}