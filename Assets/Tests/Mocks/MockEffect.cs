using Systems.Effects;
using Systems.Modifiers;

namespace Tests.Mocks
{
    public class MockEffect : IEffect
    {
        public int ApplyCount;
        public int RemoveCount;

        public override void Apply(ModifiableTarget target)
        {
            ++ApplyCount;
        }

        public override void Remove(ModifiableTarget target)
        {
            ++RemoveCount;
        }
    }
}