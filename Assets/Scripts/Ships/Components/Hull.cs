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
        public void Start()
        {
            OnDisabledChanged += delegate
            {
                if (Disabled)
                {
                    //TODO handle other parts of death
                    Destroy(gameObject); //Kill ship
                }
            };
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