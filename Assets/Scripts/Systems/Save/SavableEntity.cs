using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Systems.Save
{
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] private string id;
        public string Id => id;

        public SerializableDictionary<string, object> Save()
        {
            var savableComponents = GetComponents<ISavable>();
            var components = new SerializableDictionary<string, object>();
            foreach (var savable in savableComponents)
            {
                components[savable.id] = savable.SaveState();
            }
            return components;
        }

        public void Load(Dictionary<string, JObject> savedState)
        {
            var savableComponents = GetComponents<ISavable>();
            foreach (var savable in savableComponents)
            {
                savable.LoadState(savedState[savable.id]);
            }
        }
    }
}
