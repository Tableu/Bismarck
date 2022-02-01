using Newtonsoft.Json.Linq;

namespace Systems.Save
{
    public interface ISavable
    {
        public string id { get; }
        object SaveState();
        void LoadState(JObject state);
    }
}
