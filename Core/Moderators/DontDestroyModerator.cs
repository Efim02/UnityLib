namespace UnityLib.Core.Moderators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Core.Extensions;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Вспомогательный класс для регистрации игровых объектов, как не уничтожаемых, в ед. экземпляре.
    /// </summary>
    /// <remarks>Сохраняются "корневые" объекты, а к ним зарегистрированные типы одиночки.</remarks>
    public static class DontDestroyModerator
    {
        private static readonly Dictionary<GameObject, List<Type>> _dictionary;

        static DontDestroyModerator()
        {
            _dictionary = new Dictionary<GameObject, List<Type>>();
        }

        /// <summary>
        /// Уничтожает игровой объект или сохраняет для <see cref="UnityEngine.Object.DontDestroyOnLoad" />.
        /// </summary>
        /// <typeparam name="T"> Тип регистрируемого объекта. </typeparam>
        /// <returns> TRUE - если объект сделался синглтоном, иначе - FALSE. </returns>
        public static bool DestroyOrSave<T>(GameObject gameObject)
        {
            var type = typeof(T);
            return DestroyOrSave(gameObject, type);
        }

        /// <summary>
        /// Уничтожает игровой объект или сохраняет для <see cref="UnityEngine.Object.DontDestroyOnLoad" />.
        /// Получает корневой игровой объект.
        /// </summary>
        /// <param name="component"> Компонент. </param>
        /// <returns> TRUE - если объект сделался синглтоном, иначе - FALSE. </returns>
        public static bool DestroyOrSave(Component component)
        {
            var type = component.GetType();
            return DestroyOrSave(component.gameObject, type);
        }

        /// <summary>
        /// Уничтожает игровой объект или сохраняет для <see cref="UnityEngine.Object.DontDestroyOnLoad" />.
        /// </summary>
        public static bool DestroyOrSave(GameObject gameObject, Type type)
        {
            var rootGameObject = gameObject.transform.GetRootParent().gameObject;
            var hasRootGameObject = _dictionary.TryGetValue(rootGameObject, out var types);
            
            // Имеется ли игровой Root объект у которого уже имеется регистрируемый тип.
            if (_dictionary.FirstOrDefault(p => p.Value.Contains(type)).Key != null)
            {
                GameLogger.Warning($"Существует лишний объект {gameObject.name}:{type.Name}");
                Object.Destroy(gameObject);
                return false;
            }

            if (hasRootGameObject)
                types.Add(type);
            else
                _dictionary.Add(rootGameObject, new List<Type> { type });

            Object.DontDestroyOnLoad(rootGameObject);

            return true;
        }

        /// <summary>
        /// Долго живущий объект.
        /// </summary>
        /// <param name="component"> Компонент. </param>
        /// <returns> TRUE - если это так, иначе - FALSE. </returns>
        public static bool IsDontDestroy(Component component)
        {
            var type = component.GetType();
            var rootGameObject = component.transform.GetRootParent().gameObject;
            return _dictionary.TryGetValue(rootGameObject, out var types) &&
                   types.Any(t => t == type);
        }
    }
}