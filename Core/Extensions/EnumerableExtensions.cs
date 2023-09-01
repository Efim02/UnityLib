namespace UnityLib.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Расширение для перечислений.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Напечатать список в текст.
        /// </summary>
        /// <typeparam name="T"> Тип. </typeparam>
        /// <param name="enumerable"> Список. </param>
        /// <param name="getter">
        /// Action возвращающий текст из каждого элемента.
        /// Если null, то ToString используется.
        /// </param>
        /// <returns> Общий текст в виде строчек для каждого элемента. </returns>
        public static string ToText<T>(this IEnumerable<T> enumerable, Func<T, string> getter = null)
        {
            var text = $"{typeof(T).Name}s";
            var list = enumerable.ToList();

            if (!list.Any())
                return $"{text} - пустой";

            getter ??= item => item.ToString();
            list.ForEach(item => text += $"\n{getter.Invoke(item)}");

            return text;
        }
    }
}