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
        /// Уничтожает игровой объект или сохраняет для <see cref="Object.DontDestroyOnLoad"/>.
        /// </summary>
        public static void DestroyOrSave(MonoBehaviour monoBehaviour)
        {
            var type = monoBehaviour.GetType();
            if (_dictionary.ContainsKey(type))
            {
                Object.Destroy(monoBehaviour);
                return;
            }

            _dictionary.Add(type, monoBehaviour.gameObject);
            Object.DontDestroyOnLoad(monoBehaviour.gameObject);
        }
    }
}