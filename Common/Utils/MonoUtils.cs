namespace UnityLib.Common.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Common.Exceptions;

    /// <summary>
    /// Доп. функции для <see cref="MonoBehaviour" />.
    /// </summary>
    public static class MonoUtils
    {
        /// <summary>
        /// Получить компонент и сделать проверки.
        /// </summary>
        /// <typeparam name="T"> Тип компонента. </typeparam>
        /// <param name="monoBehaviour"> Объект. </param>
        /// <returns> Компонент. </returns>
        public static T GetComponent<T>(Component monoBehaviour) where T : Component
        {
            var component = monoBehaviour.GetComponent<T>();
            if (component is null)
                throw new ErrorFoundException($"Нет компонента {typeof(T).Name}.");

            return component;
        }

        /// <summary>
        /// Получить компонент и сделать проверки.
        /// </summary>
        /// <typeparam name="T"> Тип компонента. </typeparam>
        /// <param name="gameObject"> Объект. </param>
        /// <returns> Компонент. </returns>
        public static T GetComponent<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component is null)
                throw new ErrorFoundException($"Нет компонента {typeof(T).Name}.");

            return component;
        }

        /// <summary>
        /// Получить компоненты и сделать проверки.
        /// </summary>
        /// <typeparam name="T"> Тип компонента. </typeparam>
        /// <param name="monoBehaviour"> Объект. </param>
        /// <returns> Компонент. </returns>
        public static List<T> GetComponents<T>(Component monoBehaviour) where T : Component
        {
            var components = monoBehaviour.GetComponents<T>();
            if (components is null || !components.Any())
                throw new ErrorFoundException($"Нет компонента {typeof(T).Name}.");

            return components.ToList();
        }

        /// <summary>
        /// Объекты был ли уничтожен.
        /// </summary>
        /// <param name="monoBehaviour"> Моно объект. </param>
        /// <returns> TRUE - если и ловит исключение. </returns>
        public static bool IsDestroyed(MonoBehaviour monoBehaviour)
        {
            try
            {
                var gm = monoBehaviour.gameObject;
                var _ = gm.transform;
                return false;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Объекты был ли уничтожен.
        /// </summary>
        /// <param name="gameObject"> Игровой объект. </param>
        /// <returns> TRUE - если и ловит исключение. </returns>
        public static bool IsDestroyed(GameObject gameObject)
        {
            try
            {
                var _ = gameObject.transform;
                return false;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Компонент был уничтожен. Проверка на null и существование объекта.
        /// </summary>
        /// <param name="component"> Компонент. </param>
        /// <returns> TRUE - если и ловит исключение. </returns>
        public static bool IsDestroyed(Component component)
        {
            try
            {
                if (component is null)
                    return true;

                var _ = component.transform;
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}