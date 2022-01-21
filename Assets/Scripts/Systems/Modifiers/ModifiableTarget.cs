using System.Collections.Generic;
using UnityEngine;

namespace Systems.Modifiers
{
    public class ModifiableTarget : MonoBehaviour
    {
        private List<Modifer> _modifiers = new List<Modifer>();
        public IReadOnlyList<Modifer> Modifiers { get; }

        public void AttachModifer(Modifer modifer)
        {
            _modifiers.Add(modifer);
            StartCoroutine(modifer.Initialize());
        }

        public void RemoveModifer(Modifer modifer)
        {
            _modifiers.Remove(modifer);
        }
    }
}