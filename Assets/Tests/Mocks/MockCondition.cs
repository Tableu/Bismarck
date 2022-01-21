using System;
using Systems.Conditions;
using Systems.Modifiers;

namespace Tests.Mocks
{
    public class MockConditionRule : ConditionRule
    {
        public readonly MockCondition Condition = new MockCondition();
        public override ICondition NewBinding(IModifiableTarget target)
        {
            return Condition;
        }

        public class MockCondition : ICondition
        {
            private bool _value;

            public bool IsTrue
            {
                get => _value;
                set
                {
                    _value = value;
                    OnChange?.Invoke(value);
                }
            }

            public event Action<bool> OnChange;
        }
    }
}