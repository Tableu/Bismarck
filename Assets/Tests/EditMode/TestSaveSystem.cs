using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Systems.Save;
using UnityEngine;

namespace Tests.EditMode
{
    internal class BasicSaveable : MonoBehaviour, ISavable
    {
        public int state;

        public string id => "basic savable";
        public object SaveState()
        {
            return new SaveData
            {
                data = state
            };
        }
        public void LoadState(JObject data)
        {
            var saveData = data.ToObject<SaveData>();
            state = saveData.data;
        }

        [Serializable]
        private struct SaveData
        {
            public int data;
        }
    }

    public class TestSaveSystem
    {
        [Test]
        public void TestSavableEntity()
        {
            var saveGo = new GameObject();
            saveGo.AddComponent<BasicSaveable>().state = 42;
            var saver = saveGo.AddComponent<SavableEntity>();
            var saveData = saver.Save();

            var saveString = JsonConvert.SerializeObject(saveData);
            var loadedData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(saveString);

            var loadGo = new GameObject();
            loadGo.AddComponent<BasicSaveable>();
            var loader = loadGo.AddComponent<SavableEntity>();

            loader.Load(loadedData);

            Assert.AreEqual(42, loadGo.GetComponent<BasicSaveable>().state);
        }
    }
}
