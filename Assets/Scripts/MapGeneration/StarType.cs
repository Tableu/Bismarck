using UnityEngine;
using Utility;

namespace MapGeneration
{
    [CreateAssetMenu(fileName = "star", menuName = "Database/StarType", order = 0)]
    public class StarType : ScriptableObject
    {
        public string starClassName;
        public Color starColor;

        [MinMaxRange(1, 10)] public RangedFloat sizeRange;

        public StellarObject MakeStar()
        {
            return new StellarObject
            {
                Color = starColor,
                Size = Random.Range(sizeRange.minValue, sizeRange.maxValue),
                Classification = starClassName
            };
        }
    }
}