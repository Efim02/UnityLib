namespace UnityLib.Editor.Drawers
{
    using UnityEditor;

    using UnityEngine;

    using UnityLib.Core.Helpers.Fields;

    /// <summary>
    /// Отображение <see cref="MapList{TKey,TValue}" /> в Inspector.
    /// </summary>
    /// <remarks>
    /// Item в списке. Для отображения словаря в Inspector нужно добавить сюда тип Item.
    /// </remarks>
    public class MapListItemDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent());

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var keyRect = new Rect(position.x, position.y, position.width / 2, position.height);
            var rectPositionX = position.x + position.width / 2 + 10;
            var valueRect = new Rect(rectPositionX, position.y, position.width / 2 - 10, position.height);

            var propKey = property.FindPropertyRelative("Key");
            var propValue = property.FindPropertyRelative("Value");

            EditorGUI.PropertyField(keyRect, propKey, GUIContent.none);
            EditorGUI.PropertyField(valueRect, propValue, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}