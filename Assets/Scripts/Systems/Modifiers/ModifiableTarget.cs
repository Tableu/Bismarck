using System.Collections.Generic;
using UnityEngine;

namespace Systems.Modifiers
{
    public class ModifiableTarget : MonoBehaviour
    {
        private readonly List<Coroutine> _effectRoutines = new List<Coroutine>();
        private readonly List<Modifer> _modifiers = new List<Modifer>();
        public IReadOnlyList<Modifer> Modifiers => _modifiers;

        public void AttachModifer(Modifer modifer)
        {
            _modifiers.Add(modifer);
            StartCoroutine(modifer.OnAttach());
        }

        public void RemoveModifer(Modifer modifer)
        {
            _modifiers.Remove(modifer);
            modifer.OnRemove();
        }
    }
}