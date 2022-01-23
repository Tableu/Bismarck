using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Modifiers
{
    [Serializable]
    public class Modifer
    {
        private readonly ICondition _condition;
        public readonly ModifierData Data;

        private bool _enabled;
        private List<PeriodicEffect.PeriodicEffectInstance> _periodicEffects = new List<PeriodicEffect.PeriodicEffectInstance>();
        private ModifiableTarget _target;

        internal Modifer(ModifierData data, ModifiableTarget target)
        {
            Data = data;
            _target = target;
            if (Data.Condition != null)
            {
                _condition = Data.Condition.NewBinding(_target);
                _condition.OnChange += OnConditionChanged;
            }

            Enabled = false;
        }

        public bool Active => _condition == null || _condition.IsTrue && Enabled;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    if ((_condition == null || _condition.IsTrue) && value)
                        ActivateEffects();
                    else if (Active) DeactivateEffects();
                }

                _enabled = value;
            }
        }

        internal IEnumerator OnAttach()
        {
            Enabled = true;
            if (Data.HasDuration)
            {
                yield return new WaitForSeconds(Data.Duration);
                if (_target != null) _target.RemoveModifer(this);
            }

            yield return null;
        }

        internal void OnRemove()
        {
            if (_target != null)
            {
                if (Active) DeactivateEffects();
                _target.StartCoroutine(OnAttach());
                _target = null;
            }

            if (_condition != null) _condition.OnChange += OnConditionChanged;
        }

        private void ActivateEffects()
        {
            foreach (var effect in Data.Effects)
            {
                effect.Apply(_target);
                if (effect is PeriodicEffect periodicEffect)
                {
                    var inst = periodicEffect.CreateInstance(_target);
                    _periodicEffects.Add(inst);
                    _target.StartCoroutine(inst.Start());
                }
            }
        }

        private void DeactivateEffects()
        {
            foreach (var effect in Data.Effects)
            {
                effect.Remove(_target);
            }
            foreach (var periodicEffect in _periodicEffects)
            {
                periodicEffect.Stop();
            }
            _periodicEffects.Clear();
        }

        private void OnConditionChanged(bool newValue)
        {
            if (newValue)
                ActivateEffects();
            else
                DeactivateEffects();
        }
    }
}