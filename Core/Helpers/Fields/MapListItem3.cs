namespace UnityLib.Core.Helpers.Fields
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Элемент списка с двумя ключами.
    /// </summary>
    /// <typeparam name="TKey1"> Ключ 1. </typeparam>
    /// <typeparam name="TKey2"> Ключ 2. </typeparam>
    /// <typeparam name="TValue"> Значение. </typeparam>
    [Serializable]
    public class MapListItem3<TKey1, TKey2, TValue>
        where TKey1 : Enum
        where TKey2 : Enum
    {
        [SerializeField]
        private TValue _value;

        [SerializeField]
        private TKey1 _key1;

        [SerializeField]
        private TKey2 _key2;

        /// <summary>
        /// Первый ключ.
        /// </summary>
        public TKey1 Key1 => _key1;

        /// <summary>
        /// Второй ключ.
        /// </summary>
        public TKey2 Key2 => _key2;

        /// <summary>
        /// Значение.
        /// </summary>
        public TValue Value => _value;
    }
}