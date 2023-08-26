namespace UnityLib.Core.Extensions
{
    using TMPro;

    /// <summary>
    /// Расширение для "TMP_Text".
    /// </summary>
    public static class TextMeshProExtensions 
    {
        /// <summary>
        /// Устанавливает значение, даже если null.
        /// </summary>
        /// <param name="text">Текстовый элемент.</param>
        /// <param name="value">Значение.</param>
        /// <remarks>В TMP_Text.text нельзя устанавливать null.</remarks>
        public static void SetTextAnyOne<T>(this TMP_Text text, T value)
        {
            text.text = value?.ToString() ?? string.Empty;
        }
    }
}