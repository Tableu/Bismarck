using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Demo
{
    [CustomEditor(typeof(ModiferAdder))]
    public class ModiferAdderInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var adder = target as ModiferAdder;
            if (adder == null) return;
            if (GUILayout.Button("Add Modifer")) adder.AddSelectedModifer();
        }
    }
}