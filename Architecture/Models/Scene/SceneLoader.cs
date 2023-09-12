namespace UnityLib.Architecture.Models.Scene
{
    using System;

    using UnityEngine.SceneManagement;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.MVC;
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Загрузчик сцены.
    /// </summary>
    internal class SceneLoader : ISceneLoader
    {
        /// <summary>
        /// Текущая сцена.
        /// </summary>
        private SceneInfo _currentScene;

        /// <summary>
        /// Предыдущая сцена.
        /// </summary>
        private SceneInfo _previousScene;

        /// <summary>
        /// Утилита для работы с загрузкой уровней.
        /// </summary>
        public SceneLoader()
        {
            SceneManager.activeSceneChanged += SceneChanged;
        }

        private AutoViewModelLinker AutoViewModelLinker => Injector.Get<AutoViewModelLinker>();

        /// <inheritdoc />
        public int CurrentSceneIndex => _currentScene.Index;

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
        public int PreviousSceneIndex => _previousScene?.Index ?? 0;

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
            ExecuteUtils.SafeExecute(() =>
            {
                _previousScene = _currentScene;
                _currentScene = new SceneInfo(next);

                GameLogger.Info(
                    $"Изменена сцена: {_previousScene?.Name} -> {next.name}");
                LevelLoaded?.Invoke();

                AutoViewModelLinker.CheckSceneViews();
            });
        }
    }
}