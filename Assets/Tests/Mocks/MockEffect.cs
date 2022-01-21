using System;
using Systems.Effects;
using Systems.Modifiers;

namespace Tests.Mocks
{
    public class MockEffect : IEffect
    {
        public int ApplyCount;
        public int RemoveCount;

        public override void Apply(IModifiableTarget target)
        {
            ++ApplyCount;
        }

        public override void Remove(IModifiableTarget target)
        {
            ++RemoveCount;
        }
        
    }
}