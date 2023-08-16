namespace UnityLib.Editor.Drawers
{
    using UnityEditor;

    using UnityEngine;

    using UnityLib.Core.Helpers.Fields;

    /// <summary>
    /// Отображение контейнера с тремя параметрами.
    /// </summary>
    public class ThreeParamsDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent());

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var width = (position.width - 10) / 3;
            var propRect1 = new Rect(position.x, position.y, width, position.height);
            var propRect2 = new Rect(position.x + width + 5, position.y, width, position.height);
            var propRect3 = new Rect(position.x + width * 2 + 10, position.y, width, position.height);

            var propParam1 = property.FindPropertyRelative(nameof(ThreeParams<int, int, int>.Param1));
            var propParam2 = property.FindPropertyRelative(nameof(ThreeParams<int, int, int>.Param2));
            var propParam3 = property.FindPropertyRelative(nameof(ThreeParams<int, int, int>.Param3));

            EditorGUI.PropertyField(propRect1, propParam1, GUIContent.none);
            EditorGUI.PropertyField(propRect2, propParam2, GUIContent.none);
            EditorGUI.PropertyField(propRect3, propParam3, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}