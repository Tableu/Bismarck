using System.Collections;
using UnityEngine;

namespace Systems.Modifiers
{
    public abstract class PeriodicEffect : IEffect
    {

        [SerializeField]
        public float tickPeriod;
        public abstract PeriodicEffectInstance CreateInstance(ModifiableTarget target);

        public abstract class PeriodicEffectInstance
        {
            private readonly float _tickPeriod;
            private bool _stopped;
            protected PeriodicEffectInstance(float tickPeriod)
            {
                _tickPeriod = tickPeriod;
            }
            protected abstract void Tick();
            internal IEnumerator Start()
            {
                _stopped = false;
                while (!_stopped)
                {
                    Tick();
                    yield return new WaitForSeconds(_tickPeriod);
                }
            }
            internal void Stop()
            {
                _stopped = true;
            }
        }
    }
}
