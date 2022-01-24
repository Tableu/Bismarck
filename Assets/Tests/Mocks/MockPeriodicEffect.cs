using System;
using Systems.Modifiers;

namespace Tests.Mocks
{
    public class MockPeriodicEffect : PeriodicEffect
    {

        public int ApplyCount;
        public int RemoveCount;
        public Action Action;
        public override void Apply(ModifiableTarget target)
        {
            ++ApplyCount;
        }
        public override void Remove(ModifiableTarget target)
        {
            ++RemoveCount;
        }
        public override PeriodicEffectInstance CreateInstance(ModifiableTarget target)
        {
            return new Impl(tickPeriod, Action);
        }


        private class Impl : PeriodicEffectInstance
        {

            private readonly Action _onTick;
            public Impl(float tickPeriod, Action onTick) : base(tickPeriod)
            {
                _onTick = onTick;
            }
            protected override void Tick()
            {
                _onTick();
            }
        }
    }
}
