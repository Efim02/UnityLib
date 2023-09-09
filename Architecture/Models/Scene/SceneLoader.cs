namespace UnityLib.Architecture.Models.Scene
{
    using System;

    using UnityEngine.SceneManagement;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.MVC;

    /// <summary>
    /// Загрузчик сцены.
    /// </summary>
    internal class SceneLoader : ISceneLoader
    {
        /// <summary>
        /// Утилита для работы с загрузкой уровней.
        /// </summary>
        public SceneLoader()
        {
            SceneManager.activeSceneChanged += SceneChanged;
        }

        private AutoViewModelLinker AutoViewModelLinker => Injector.Get<AutoViewModelLinker>();

        /// <inheritdoc />
        public int CurrentSceneIndex { get; private set; }

        /// <inheritdoc />
        public TScene GetCurrentScene<TScene>() where TScene : struct
        {
            return GetScene<TScene>(CurrentSceneIndex);
        }

        /// <inheritdoc />
        public TScene GetPreviousScene<TScene>() where TScene : struct
        {
            return GetScene<TScene>(PreviousSceneIndex);
        }

        /// <summary>
        /// Уровень загружен.
        /// </summary>
        public event Action LevelLoaded;

        /// <summary>
        /// Уровень будет загружен.
        /// </summary>
        public event Action LevelLoading;

        /// <inheritdoc />
        public void LoadScene<TScene>(TScene scene) where TScene : Enum
        {
            var sceneEnum = typeof(TScene);
            var sceneEnumValues = Enum.GetNames(sceneEnum);
            var gameIndex = Array.IndexOf(sceneEnumValues, scene.ToString());

            if (gameIndex == -1)
                throw new Exception($"Перечисление {typeof(TScene)} не содержит {scene}");

            LoadScene(gameIndex);
        }

        /// <inheritdoc />
        public void LoadScene(int gameLevelIndex)
        {
            try
            {
                Injector.ClearSceneInstances();

                LevelLoading?.Invoke();

                SceneManager.LoadScene(gameLevelIndex);
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception, "Ошибка загрузки уровня");
            }
        }

        /// <inheritdoc />
        public int PreviousSceneIndex { get; private set; }

        /// <summary>
        /// Получает сцену по индексу.
        /// </summary>
        /// <param name="sceneIndex"> Индекс. </param>
        /// <typeparam name="TScene"> Тип. </typeparam>
        /// <returns> Сцена. </returns>
        private TScene GetScene<TScene>(int sceneIndex) where TScene : struct
        {
            var sceneEnum = typeof(TScene);
            var sceneEnumValues = Enum.GetNames(sceneEnum);

            if (sceneEnumValues.Length < sceneIndex ||
                !Enum.TryParse<TScene>(sceneEnumValues[sceneIndex], out var scene))
                throw new Exception("Текущий индекс не определен в перечислении");

            return scene;
        }

        /// <summary>
        /// Сцена изменена.
        /// </summary>
        /// <param name="previous"> Предыдущая. </param>
        /// <param name="next"> Текущая. </param>
        private void SceneChanged(Scene previous, Scene next)
        {
            PreviousSceneIndex = CurrentSceneIndex;
            CurrentSceneIndex = next.buildIndex;

            GameLogger.Info($"Изменена сцена: {previous.name} -> {next.name}");
            LevelLoaded?.Invoke();

            AutoViewModelLinker.CheckSceneViews();
        }
    }
}