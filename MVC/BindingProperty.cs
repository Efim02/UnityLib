namespace UnityLib.MVC
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Свойство с привязкой.
    /// </summary>
    /// <typeparam name="T"> Тип значения. </typeparam>
    public class BindingProperty<T>
    {
        [SerializeField]
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(_value);
            }
        }

        public static BindingProperty<T> operator +(BindingProperty<T> bindingProperty, Action<T> action)
        {
            bindingProperty.PropertyChanged += action;
            return bindingProperty;
        }

        public static BindingProperty<T> operator -(BindingProperty<T> bindingProperty, Action<T> action)
        {
            bindingProperty.PropertyChanged -= action;
            return bindingProperty;
        }

        public event Action<T> PropertyChanged;
    }
}