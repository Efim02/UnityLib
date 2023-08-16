namespace UnityLib.Core.Managers
{
    using System;

    using UnityEngine;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;

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

        /// <summary>
        /// Метод вызываемый при завершении приложения.
        /// </summary>
        private static void AppQuit()
        {
            GameLogger.Save();

            Quiting?.Invoke();
        }
    }
}