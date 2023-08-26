namespace UnityLib.Core.Helpers.Json
{
    using System;

    /// <summary>
    /// Ключ-значение.
    /// </summary>
    [Serializable]
    public class KeyValue<TKey, TValue> : IEquatable<KeyValue<TKey, TValue>>
        where TKey : IEquatable<TKey>
    {
        private TKey _key;
        private TValue _value;

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

        /// <inheritdoc />
        public bool Equals(KeyValue<TKey, TValue> other)
        {
            return other != null && Key.Equals(other.Key);
        }
    }
}