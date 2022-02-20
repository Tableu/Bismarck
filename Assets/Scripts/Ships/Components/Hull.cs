using System;
using Newtonsoft.Json.Linq;
using Systems.Save;
using UnityEngine;

namespace Ships.Components
{
    /// <summary>
    ///     Stores ship health and manages receiving damage.
    /// </summary>
    public class Hull : DamageableComponentInfo, ISavable
    {
        protected override void DisableComponent()
        {
            if (Health <= 0)
            {
                //TODO handle other parts of death
                Destroy(gameObject); //Kill ship
            }
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
            TakeDamage(new Damage(this, 10, 100));
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