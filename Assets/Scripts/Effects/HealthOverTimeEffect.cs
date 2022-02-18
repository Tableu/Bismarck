using Ships.Components;
using Systems.Modifiers;
using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(fileName = "New Health Over Time Effect", menuName = "Effects/Health Over Time", order = 0)]
    public class HealthOverTimeEffect : PeriodicEffect
    {
        public float baseDamage;
        public override void Apply(ModifiableTarget target)
        {
        }
        public override void Remove(ModifiableTarget target)
        {
        }
        public override PeriodicEffectInstance CreateInstance(ModifiableTarget target)
        {
            return new Impl(target.GetComponent<ShipHealth>(), tickPeriod, baseDamage);
        }

        private class Impl : PeriodicEffectInstance
        {
            private readonly float _baseDamage;
            private readonly ShipHealth _health;

            public Impl(ShipHealth health, float tickPeriod, float baseDamage) : base(tickPeriod)
            {
                _health = health;
                _baseDamage = baseDamage;
            }
            protected override void Tick()
            {
                _health.TakeDamage(new Damage(_health, _baseDamage, 100));
                Debug.Log("Dealing Damage");
            }
        }
    }
}
