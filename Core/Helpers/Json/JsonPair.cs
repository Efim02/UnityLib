namespace UnityLib.Core.Helpers.Json
{
    using System;

    /// <summary>
    /// Ключ-значение для Json.
    /// </summary>
    [Serializable]
    public class JsonPair<TKey, TValue>
    {
        private TValue _value;
        private TKey _key;

        public TKey Key
        {
            get => _key;
            set => _key = value;
        }

        public TValue Value
        {
            get => _value;
            set => _value = value;
        }
    }
}