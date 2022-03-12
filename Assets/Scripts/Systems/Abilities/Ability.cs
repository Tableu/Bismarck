using System;
using System.Collections;
using Ships.Components;
using Systems.Modifiers;
using UI.InfoWindow;
using UnityEngine;

namespace Systems.Abilities
{
    public class Ability : CooldownAbility
    {
        private AbilityData _data;

        public AbilityData Data => _data;

        public override ButtonData ButtonData => _data.ButtonData;

        public Ability(AbilityData data)
        {
            _data = data;
            CooldownMultiplier.UpdateBaseValue(data.Cooldown);
        }

        public override bool Fire(ShipStats attacker)
        {
            foreach (ModifierData modifier in Data.Modifiers)
            {
                modifier.AttachNewModifer(attacker);
            }

            return true;
        }
    }

    public abstract class CooldownAbility
    {
        private bool _onCooldown;
        public bool OnCooldown => _onCooldown;
        public ModifiableStat CooldownMultiplier { get; } = new ModifiableStat(0);
        public abstract ButtonData ButtonData { get; }
        public IEnumerator CooldownTimer()
        {
            float startTime = Time.time;
            _onCooldown = true;
            while (Time.time - startTime < CooldownMultiplier)
            {
                float percentage = Mathf.Min((Time.time - startTime) / CooldownMultiplier, 1);
                CooldownEvent?.Invoke(percentage);
                yield return null;
            }

            _onCooldown = false;
        }

        public abstract bool Fire(ShipStats attacker);

        public Action<float> CooldownEvent;
    }
}