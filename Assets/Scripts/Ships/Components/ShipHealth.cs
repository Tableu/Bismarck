using System;
using Newtonsoft.Json.Linq;
using Systems.Save;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores ship health and manages receiving damage.
    /// </summary>
    public class ShipHealth : MonoBehaviour, IDamageable, ISavable
    {
        /// <summary>
        ///     Keeps track of if health has changed this frame.
        ///     Used so that OnHealthChanged is only invoked once per frame.
        /// </summary>
        private bool _healthDirty;
        private ShipInfo _info;

        public float Health => PercentHealth * _info.MaxHealth;
        public float PercentHealth { get; private set; } = 1f;
        public Subsystem Subsystem { get; private set; } = Subsystem.Hull;

        private void Awake()
        {
            _info = GetComponent<ShipInfo>();
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

        public bool DestroyProjectile(CollisionType type)
        {
            return true;
        }
        public string id => "health";
        public object SaveState()
        {
            return new SaveData
            {
                CurrentHealth = PercentHealth
            };
        }
        public void LoadState(JObject state)
        {
            var saveData = state.ToObject<SaveData>();
            PercentHealth = saveData.CurrentHealth;
        }

        [ContextMenu("Test Damage")]
        public void TestDamage()
        {
            TakeDamage(new Damage(Vector2.down, 1, CollisionType.energy));
        }

        [ContextMenu("Repair")]
        public void Repair()
        {
            PercentHealth = 1;
            OnHealthChanged?.Invoke();
        }

        public event Action OnHealthChanged;

        [Serializable]
        private struct SaveData
        {
            public float CurrentHealth;
        }
    }
}
