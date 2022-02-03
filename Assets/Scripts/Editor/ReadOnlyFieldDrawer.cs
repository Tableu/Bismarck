using UnityEditor;
using UnityEngine;

namespace Editor
{

    /// <summary>
    ///     Attribute  used to make a field readonly in the inspector
    /// </summary>
    public class ReadOnlyField : PropertyAttribute
    {
    }

    /// <summary>
    ///     Custom drawer for read only attribute
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyField))]
    public class ReadOnlyFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var oldState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = oldState;
        }
    }
}
