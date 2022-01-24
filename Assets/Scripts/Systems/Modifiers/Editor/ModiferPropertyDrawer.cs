using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Systems.Modifiers
{
    [CustomPropertyDrawer(typeof(Modifer))]
    public class ModiferPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}