using System;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores ship component health and manages receiving damage.
    /// </summary>
    public abstract class DamageableComponentInfo : MonoBehaviour
    {
        /// <summary>
        ///     Keeps track of if health has changed this frame.
        ///     Used so that OnHealthChanged is only invoked once per frame.
        /// </summary>
        private bool _healthDirty;

        private float _maxHealth;
        private float _maxDodgeChance;
        protected ShipInfo _info;
        
        public ShipInfo ShipInfo => _info;
        public float Health => PercentHealth * _maxHealth;
        public float DodgeChance => PercentDodgeChance * _maxDodgeChance;
        public float PercentHealth { get; protected set; } = 1f;
        public float PercentDodgeChance { get; protected set; } = 1f;
        public Subsystem Subsystem { get; protected set; } = Subsystem.Hull;

        public void Init(float maxHealth, float maxDodgeChance, Subsystem subsystem)
        {
            Subsystem = subsystem;
            _maxHealth = maxHealth;
            _maxDodgeChance = maxDodgeChance;
        }
        private void Awake()
        {
            _info = GetComponent<ShipInfo>();
            OnHealthChanged += DisableComponent;
        }
        private void Update()
        {
            if (_healthDirty)
            {
                _healthDirty = false;
                OnHealthChanged?.Invoke();
            }
        }
        public void TakeDamage(Damage dmg)
        {
            PercentHealth -= dmg.RawDamage / _info.MaxHealth;
            PercentHealth = Mathf.Min(PercentHealth, 1);
            _healthDirty = true;
            if (PercentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void DisableComponent();

        [ContextMenu("Test Damage")]
        public void TestDamage()
        {
            TakeDamage(new Damage(this, 10, 100));
        }

        [ContextMenu("Repair")]
        public void Repair()
        {
            PercentHealth = 1;
            OnHealthChanged?.Invoke();
        }
        
        public event Action OnHealthChanged;
    }
}