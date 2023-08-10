namespace UnityLib.Common.Utils
{
    using System;

    using UnityEngine;

    using UnityLib.Di;

    /// <summary>
    /// Управляющий приложением.
    /// </summary>
    public class AppManager : MonoBehaviour
    {
        public void Awake()
        {
            Injector.RebindSingleton(this, false);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Вызываем так, иначе не отработает в Editor.
        /// </summary>
        private void OnApplicationQuit()
        {
            AppQuit();
        }
#endif
        public static void Initialize()
        {
            Application.quitting += AppQuit;
            Application.unloading += AppQuit;
        }

        /// <summary>
        /// Выход из приложение.
        /// </summary>
        public static event Action Quiting;

        private static void AppQuit()
        {
            Quiting?.Invoke();
        }
    }
}