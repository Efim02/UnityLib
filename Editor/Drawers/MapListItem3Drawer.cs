namespace UnityLib.Editor.Drawers
{
    using UnityEditor;

    using UnityEngine;

    using UnityLib.Core.Helpers.Fields;

    /// <summary>
    /// Отображение <see cref="MapList3{TKey1,TKey2,TValue}" /> в Inspector.
    /// </summary>
    /// <remarks>
    /// Item в списке. Для отображения словаря в Inspector нужно добавить сюда тип Item.
    /// </remarks>
    public class MapListItem3Drawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent());

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var width = position.width / 3;
            var key1Rect = new Rect(position.x, position.y, width, position.height);
            var key2Rect = new Rect(position.x + width + 10, position.y, width, position.height);
            var valueRect = new Rect(position.x + width * 2 + 20, position.y, width - 30, position.height);

            var propKey1 = property.FindPropertyRelative("Key1");
            var propKey2 = property.FindPropertyRelative("Key2");
            var propValue = property.FindPropertyRelative("Value");

            EditorGUI.PropertyField(key1Rect, propKey1, GUIContent.none);
            EditorGUI.PropertyField(key2Rect, propKey2, GUIContent.none);
            EditorGUI.PropertyField(valueRect, propValue, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}