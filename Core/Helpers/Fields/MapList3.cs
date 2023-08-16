namespace UnityLib.Core.Helpers.Fields
{
    using System;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Architecture.Log;

    /// <summary>
    /// Словарь с двумя ключами.
    /// </summary>
    /// <typeparam name="TKey1"> Ключ 1. </typeparam>
    /// <typeparam name="TKey2"> Ключ 2. </typeparam>
    /// <typeparam name="TValue"> Значение. </typeparam>
    /// <remarks>
    /// Использовать только для указания в UI.
    /// </remarks>
    [Serializable]
    public class MapList3<TKey1, TKey2, TValue>
        where TKey1 : Enum
        where TKey2 : Enum
        where TValue : class
    {
        [SerializeField]
        private MapListItem3<TKey1, TKey2, TValue>[] _items;

        /// <summary>
        /// Элементы.
        /// </summary>
        public MapListItem3<TKey1, TKey2, TValue>[] Items => _items;

        /// <summary>
        /// Количество.
        /// </summary>
        public int Count => Items.Length;

        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns> Значение. Null если нет. </returns>
        public TValue GetValue(TKey1 key1, TKey2 key2)
        {
            return Items.FirstOrDefault(i1 => i1.Key1.Equals(key1) && i1.Key2.Equals(key2))!.Value;
        }

        /// <summary>
        /// Попробовать получить значение.
        /// </summary>
        /// <returns> Значение. Null если нет. </returns>
        public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
        {
            value = GetValue(key1, key2);
            if (value is null)
                GameLogger.Error($"По ключу {key1}-{key2}, не значения.");

            return value is not null;
        }
    }
}