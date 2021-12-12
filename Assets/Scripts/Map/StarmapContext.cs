using System;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "context", menuName = "Shared Resource/Starmap Context", order = 0)]
    public class StarmapContext : ScriptableObject
    {
        // todo: make serializable
        private Starmap _starmap;

        // public Starmap Starmap
        // {
        //     get
        //     {
        //         _starmap ??= new Starmap(10);
        //         return _starmap;
        //     }
        //     private set => _starmap = value;
        // }
        public StarSystem CurrentStarSystem { get; } = null;
    }
}