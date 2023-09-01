namespace UnityLib.Core.Moderators
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Вспомогательный класс для регистрации игровых объектов, как не уничтожаемых, в ед. экземпляре.
    /// </summary>
    public static class DontDestroyModerator
    {
        private static Dictionary<Type, GameObject> _dictionary;

        static DontDestroyModerator()
        {
            _dictionary = new Dictionary<Type, GameObject>();
        }

        /// <summary>
        /// Уничтожает игровой объект или сохраняет для <see cref="Object.DontDestroyOnLoad" />.
        /// </summary>
        /// <returns>TRUE - если объект сделался синглтоном, иначе - FALSE.</returns>
        public static bool DestroyOrSave<T>(GameObject gameObject)
        {
            var type = typeof(T);
            return DestroyOrSave(gameObject, type);
        }

        /// <summary>
        /// Уничтожает игровой объект или сохраняет для <see cref="Object.DontDestroyOnLoad" />.
        /// </summary>
        /// <param name="component">Компонент.</param>
        /// <returns>TRUE - если объект сделался синглтоном, иначе - FALSE.</returns>
        public static bool DestroyOrSave(Component component)
        {
            var type = component.GetType();
            var gameObject = component.gameObject;

            return DestroyOrSave(gameObject, type);
        }
        
        /// <summary>
        /// Уничтожает игровой объект или сохраняет для <see cref="Object.DontDestroyOnLoad" />.
        /// </summary>
        private static bool DestroyOrSave(GameObject gameObject, Type type)
        {
            if (gameObject.transform.parent != null)
                throw new Exception($"{gameObject.name} не является root объектом сцены");

            if (_dictionary.ContainsKey(type))
            {
                Object.Destroy(gameObject);
                return false;
            }

            _dictionary.Add(type, gameObject);
            Object.DontDestroyOnLoad(gameObject);

            return true;
        }
    }
}