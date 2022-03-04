using System;
using System.Collections;
using UnityEngine;

namespace Systems.Abilities
{
    public class Ability
    {
        private AbilityData _abilityData;
        private bool _onCooldown;

        public AbilityData AbilityData => _abilityData;
        public bool OnCooldown => _onCooldown;

        public Ability(AbilityData abilityData)
        {
            _abilityData = abilityData;
        }

        public IEnumerator CooldownTimer()
        {
            float startTime = Time.time;
            _onCooldown = true;
            while (Time.time - startTime < _abilityData.Cooldown)
            {
                float percentage = Mathf.Min((Time.time - startTime) / _abilityData.Cooldown, 1);
                CooldownEvent?.Invoke(percentage);
                yield return null;
            }

            _onCooldown = false;
        }

        public Action<float> CooldownEvent;
    }
}