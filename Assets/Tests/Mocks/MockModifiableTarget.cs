using System;
using System.Collections.Generic;
using Systems.Modifiers;
using UnityEngine;

namespace Tests.Mocks
{
    public class MockModifiableTarget: MonoBehaviour, IModifiableTarget
    {
        public  List<Modifer> Modifiers = new List<Modifer>();
        public void AttachModifer(Modifer modifer)
        {
            Modifiers.Add(modifer);
            StartCoroutine(modifer.Initialize());
        }

        public void RemoveModifer(Modifer modifer)
        {
            Modifiers.Remove(modifer);
        }
    }
}