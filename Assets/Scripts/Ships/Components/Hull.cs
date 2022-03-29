using System;
using Newtonsoft.Json.Linq;
using Systems.Save;
using Subsystem = UI.InfoWindow.Subsystem;

namespace Ships.Components
{
    /// <summary>
    ///     Stores ship health and manages receiving damage.
    /// </summary>
    public class Hull : DamageableComponent, ISavable
    {
        public void Start()
        {
            Subsystem = Subsystem.Hull;
            OnDisabledChanged += delegate(bool disabled)
            {
                if (disabled)
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