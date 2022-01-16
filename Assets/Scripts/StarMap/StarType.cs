using UnityEngine;

namespace StarMap
{
    [CreateAssetMenu(fileName = "New Star Type", menuName = "Map/Star Type", order = 0)]
    public class StarType : ScriptableObject
    {
        public Sprite MapImage;
    }
}