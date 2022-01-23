using UnityEngine;

namespace DefaultNamespace.Systems
{
    [CreateAssetMenu(fileName = "New Tag", menuName = "Tag Reference", order = 0)]
    public class TagReference : ScriptableObject
    {
        public string Tag;
    }
}