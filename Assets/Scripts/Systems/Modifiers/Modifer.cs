using System.Collections;
using System.Linq;
using Systems.Conditions;
using UnityEngine;

namespace Systems.Modifiers
{
    public class Modifer
    {
        private readonly ICondition _condition;
        private readonly ModifierData _data;

        private bool _enabled;
        private ModifiableTarget _target;

        public Modifer(ModifierData data, ModifiableTarget target)
        {
            _data = data;
            _target = target;
            if (_data.Condition != null)
            {
                _condition = _data.Condition.NewBinding(_target);
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

        public IEnumerator Initialize()
        {
            Enabled = true;
            if (_data.HasDuration)
            {
                yield return new WaitForSeconds(_data.Duration);
                Remove();
            }

            yield return null;
        }

        public void Remove()
        {
            if (_target != null)
            {
                if (Active) DeactivateEffects();
                _target.RemoveModifer(this);
                _target = null;
            }

            if (_condition != null) _condition.OnChange += OnConditionChanged;
        }

        private void ActivateEffects()
        {
            foreach (var effect in _data.EffectGroups.SelectMany(effectGroup => effectGroup.Effects))
                effect.Apply(_target);
        }

        private void DeactivateEffects()
        {
            foreach (var effect in _data.EffectGroups.SelectMany(effectGroup => effectGroup.Effects))
                effect.Remove(_target);
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