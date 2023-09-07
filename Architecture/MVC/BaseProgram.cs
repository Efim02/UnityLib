namespace UnityLib.Architecture.MVC
{
    using System;

    using UnityEngine;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;
    using UnityLib.Core.Models.Level;
    using UnityLib.Core.Utils;

    /// <summary>
    /// Абстрактный класс для запуска приложения.
    /// </summary>
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
        /// <remarks> Callback.<br/>Для работы программы необходимо зарегистрировать: <see cref="ILevelChanger" />. </remarks>
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
            if (!Injector.TryGet<ILevelChanger>(out var levelChanger) ||
                !levelChanger.GetType().BaseType!.Name.Contains("BaseLevelChanger"))
            {
                throw new Exception(
                    $"Не инициализирован {nameof(ILevelChanger)}, необходимо проверить наследника \"ILevelChanger\"");
            }
        }
    }
}