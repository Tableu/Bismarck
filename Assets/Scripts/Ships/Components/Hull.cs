using System;
using Newtonsoft.Json.Linq;
using Scene;
using Systems.Save;
using UnityEngine;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores ship health and manages receiving damage.
    /// </summary>
    public class Hull : DamageableComponentInfo, ISavable
    {
        public void Start()
        {
            Subsystem = Subsystem.Hull;
            OnDisabledChanged += delegate
            {
                if (Disabled)
                {
                    //TODO handle other parts of death
                    Destroy(gameObject); //Kill ship
                }
            };
            GameObject parent = GameObject.FindWithTag("HealthBars");
            if (parent != null)
            {
                GameObject healthBar = Instantiate(_info.Data.HealthBar, parent.transform);
                HealthBar script = healthBar.GetComponent<HealthBar>();
                if (script != null)
                {
                    script.Bind(this);
                }
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

        [Serializable]
        private struct SaveData
        {
            public float CurrentHealth;
        }
    }
}