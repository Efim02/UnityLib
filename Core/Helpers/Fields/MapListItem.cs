namespace UnityLib.Core.Helpers.Fields
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Элемент <see cref="MapList{TKey, TValue}" /> редактируемого в UI.
    /// </summary>
    /// <typeparam name="TKey"> Ключ. </typeparam>
    /// <typeparam name="TValue"> Значение. </typeparam>
    [Serializable]
    public class MapListItem<TKey, TValue>
        where TValue : class
    {
        [SerializeField]
        private TValue _value;

        [SerializeField]
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