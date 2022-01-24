using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Systems.Modifiers
{
    [CustomEditor(typeof(ModifiableTarget))]
    public class ModifiableTargetEditor : Editor
    {
        private bool _showPosition = true;
        private string _status = "Active Modifiers";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var modifiableTarget = target as ModifiableTarget;
            if (modifiableTarget == null) return;

            _status = $"{modifiableTarget.Modifiers.Count} Active Modifiers";
            _showPosition = EditorGUILayout.Foldout(_showPosition, _status);
            if (_showPosition)
            {
                EditorGUI.indentLevel += 1;
                GUI.enabled = false;
                foreach (var modifier in modifiableTarget.Modifiers)
                {
                    // todo: show more information
                    EditorGUILayout.TextField(modifier.Data.ModiferName);
                }

                GUI.enabled = true;
                EditorGUI.indentLevel -= 1;
            }
        }
    }
}