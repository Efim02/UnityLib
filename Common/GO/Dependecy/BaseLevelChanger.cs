namespace UnityLib.Common.GO.Dependecy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine.SceneManagement;

    using UnityLib.Common.GO.Logger;

    /// <summary>
    /// Изменитель уровней.
    /// </summary>
    /// <typeparam name="TLevel"> Тип перечисления уровней. </typeparam>
    public abstract class BaseLevelChanger<TLevel> : ILevelChanger where TLevel : Enum
    {
        /// <summary>
        /// Список наименований перечислений.
        /// </summary>
        private readonly List<string> _levelEnumNames;

        /// <summary>
        /// Утилита для работы с загрузкой уровней.
        /// </summary>
        public BaseLevelChanger()
        {
            _levelEnumNames = Enum.GetNames(typeof(TLevel)).ToList();
            SceneManager.activeSceneChanged += SceneChanged;
        }

        /// <summary>
        /// Текущий уровень
        /// </summary>
        public TLevel CurrentLevel { get; private set; }

        /// <summary>
        /// Предыдущая уровень.
        /// </summary>
        public TLevel PreviousLevel { get; private set; }

        /// <summary>
        /// Уровень загружен.
        /// </summary>
        public event Action LevelLoaded;

        /// <summary>
        /// Уровень загружается.
        /// </summary>
        public event Action LevelLoading;

        /// <summary>
        /// Загрузить уровень.
        /// </summary>
        /// <param name="gameLevel"> Уровень. </param>
        public void LoadLevel<TLevelEnum>(TLevelEnum gameLevel) where TLevelEnum : Enum
        {
            try
            {
                LevelLoading?.Invoke();
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception, "Ошибка загрузки уровня.");
            }

            var gameIndex = _levelEnumNames.IndexOf(gameLevel.ToString());
            SceneManager.LoadScene(gameIndex);
        }

        /// <summary>
        /// Сцена изменена.
        /// </summary>
        /// <param name="previous"> Предыдущая. </param>
        /// <param name="next"> Текущая. </param>
        private void SceneChanged(Scene previous, Scene next)
        {
            PreviousLevel = CurrentLevel;

            var levelEnumName = _levelEnumNames[next.buildIndex];
            var enumValue = Enum.Parse(typeof(TLevel), levelEnumName, true);
            CurrentLevel = (TLevel)enumValue;

            GameLogger.Info($"Изменена сцена: {PreviousLevel} -> {CurrentLevel}");
            LevelLoaded?.Invoke();
        }
    }
}