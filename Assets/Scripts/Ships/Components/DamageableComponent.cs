using System;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores ship component health and manages receiving damage.
    /// </summary>
    public abstract class DamageableComponent : MonoBehaviour
    {
        /// <summary>
        ///     Keeps track of if health has changed this frame.
        ///     Used so that OnHealthChanged is only invoked once per frame.
        /// </summary>
        private bool _healthDirty;

        private float _maxHealth;
        private float _maxDodgeChance;
        private float _disableStart;
        private bool _disabled;

        protected ShipStats Stats;

        public bool Disabled => _disabled;
        public ShipStats ShipStats => Stats;
        public float Health => PercentHealth * _maxHealth;
        public float DodgeChance => PercentDodgeChance * _maxDodgeChance;
        public float PercentHealth { get; protected set; } = 1f;
        public float PercentDodgeChance { get; protected set; } = 1f;
        public Subsystem Subsystem { get; protected set; }

        public void SetData(float maxHealth, float maxDodgeChance, Subsystem subsystem)
        {
            Subsystem = subsystem;
            _maxHealth = maxHealth;
            _maxDodgeChance = maxDodgeChance;
        }
        private void Awake()
        {
            Stats = GetComponent<ShipStats>();
        }
        private void Update()
        {
            if (_healthDirty)
            {
                _healthDirty = false;
                OnHealthChanged?.Invoke();
            }

            if (_disabled)
            {
                if (Time.time - _disableStart > Stats.RepairTimeMultiplier)
                {
                    _disabled = false;
                    PercentHealth = 1;
                    OnDisabledChanged?.Invoke(false);
                }
            }
        }

        public void TakeDamage(float damage)
        {
            PercentHealth -= damage / _maxHealth;
            PercentHealth = Mathf.Min(PercentHealth, 1);
            _healthDirty = true;
            if (Health <= 0.01)
            {
                _disabled = true;
                _disableStart = Time.time;
                OnDisabledChanged?.Invoke(true);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Test Damage")]
        public void TestDamage()
        {
            TakeDamage(10);
        }

        [ContextMenu("Repair")]
        public void Repair()
        {
            PercentHealth = 1;
            OnHealthChanged?.Invoke();
        }
#endif
        
        public event Action OnHealthChanged;
        public event Action<bool> OnDisabledChanged;
    }
}