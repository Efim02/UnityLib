namespace UnityLib.Architecture.MVC
{
    using UnityEngine;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Models.Scene;
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Абстрактный класс для запуска приложения.
    /// </summary>
    /// <remarks> Инициализация программы происходит единожды. </remarks>
    public abstract class BaseProgram : MonoBehaviour
    {
        private static bool _isInitialized;

        /// <summary>
        /// Инициализирован.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        protected void Awake()
        {
            ExecuteUtils.SafeExecute(() =>
            {
                if (_isInitialized)
                    return;

                Injector.RebindSingleton<AutoViewModelLinker>(false);
                Injector.RebindSingleton<ISceneLoader, SceneLoader>(false);

                AwakeProgram();
                ValidateAwakeProgram();
            });
        }

        protected void Start()
        {
            ExecuteUtils.SafeExecute(() =>
            {
                if (_isInitialized)
                    return;

                StartProgram();
                Injector.RebindSingleton(GetType(), this, false);
                GameLogger.Info($"Зарегистрирована программа {GetType()}");
                // TODO: проверить что за тип, должен быть наследник.
                _isInitialized = true;
            });
        }

        /// <summary>
        /// Инициализация программы.
        /// </summary>
        /// <remarks> Callback.<br /> </remarks>
        protected virtual void AwakeProgram()
        {
        }

        /// <summary>
        /// Запуск программы.
        /// </summary>
        /// <remarks> Callback. </remarks>
        protected virtual void StartProgram()
        {
        }

        /// <summary>
        /// Валидирует запуск программы.
        /// </summary>
        private void ValidateAwakeProgram()
        {
        }
    }
}