namespace UnityLib.Core.Helpers.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    /// Словарь редактируемый в UI MapListItemDrawer.
    /// </summary>
    /// <typeparam name="TKey"> Key. </typeparam>
    /// <typeparam name="TValue"> Value </typeparam>
    [Serializable]
    public class MapList<TKey, TValue>
        where TValue : class
    {
        [SerializeField]
        private List<MapListItem<TKey, TValue>> _items;

        /// <summary>
        /// Элементы.
        /// </summary>
        public List<MapListItem<TKey, TValue>> Items => _items;

        /// <summary>
        /// Количество.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Свойство индексатор, вернет через GET. Через метод SET будет установлено значение даже если его нет в словаре.
        /// </summary>
        /// <param name="key"> Ключ. </param>
        /// <returns> объект TValue (который может быть Null) </returns>
        /// <exception cref="ArgumentException"> Если ключа не существует. </exception>
        public TValue this[TKey key]
        {
            get
            {
                if (!TryGetItem(key, out var value))
                    throw new ArgumentException($"Нет такого ключа {key}.");

                return value?.Value;
            }
            set
            {
                if (!TryGetItem(key, out var item))
                {
                    item = new MapListItem<TKey, TValue> { Key = key };
                    Items.Add(item);
                }

                item.Value = value;
            }
        }

        /// <summary>
        /// Все значения.
        /// </summary>
        public IEnumerable<TValue> Values => Items.Select(item => item.Value);

        /// <summary>
        /// Попробовать получить значение.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var item in Items)
            {
                if (!item.Key.Equals(key))
                    continue;

                value = item.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Получить Item в словаре по ключу.
        /// </summary>
        /// <param name="key"> Ключ. </param>
        /// <param name="mapListItem"> Item. </param>
        /// <returns> TRUE - если есть в словаре. </returns>
        private bool TryGetItem(TKey key, out MapListItem<TKey, TValue> mapListItem)
        {
            foreach (var item in Items)
            {
                if (!item.Key.Equals(key))
                    continue;

                mapListItem = item;
                return true;
            }

            mapListItem = null;
            return false;
        }
    }
}