namespace UnityLib.Core.Helpers.Json
{
    using System;

    /// <summary>
    /// Класс хранящйи поле с множеством объектов.
    /// </summary>
    [Serializable]
    public class JsonArray<T>
    {
        private T[] _items;

        /// <summary>
        /// Множество  объектов.
        /// </summary>
        public T[] Items
        {
            get => _items;
            set => _items = value;
        }
    }
}