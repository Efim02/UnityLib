namespace UnityLib.Architecture.MVC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Utils;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Связыватель конкретной одной модели и ее представлений.
    /// </summary>
    internal class AutoViewModelLinker
    {
        /// <summary>
        /// Словарь, где ключ - тип модели, а значение - модель и связанные представления.
        /// </summary>
        private readonly Dictionary<Type, AutoViewModelLink> _dictionary;

        /// <summary>
        /// Связыватель конкретной одной модели и ее представлений.
        /// </summary>
        public AutoViewModelLinker()
        {
            _dictionary = new Dictionary<Type, AutoViewModelLink>();
        }

        /// <summary>
        /// Добавить модель.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="model"> Модель. </param>
        public void AddModel<TModel>(TModel model) where TModel : SingleModel
        {
            var modelType = model.GetType();
            if (_dictionary.ContainsKey(modelType))
                GameLogger.Warning($"Модель {modelType.Name} уже зарегистрирована; синглтон.");

            _dictionary.Add(modelType, new AutoViewModelLink(modelType, model));
        }

        /// <summary>
        /// Проверяет все представления сцены.
        /// </summary>
        /// <remarks> Обнаруженные представления на сцене добавляются в список представлений. </remarks>
        public void CheckSceneViews()
        {
            CheckPreviousSceneViews();

            var allObjects = Object.FindObjectsOfType<GameObject>();
            var autoViews = allObjects.SelectMany(o => o.GetComponents<IAutoView>()).ToList();

            autoViews.ForEach(AddView);
        }

        /// <summary>
        /// Удалить модель.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="model"> Модель. </param>
        public void RemoveModel<TModel>(TModel model) where TModel : SingleModel
        {
            var modelType = model.GetType();

            var autoViewModelLink = _dictionary[modelType];
            autoViewModelLink.Views.ForEach(a => a.Destruct());

            _dictionary.Remove(modelType);
        }

        /// <summary>
        /// Получить представления, для модели.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="views"> Представления. </param>
        /// <param name="model"> Модель. </param>
        /// <returns> TRUE - если есть представления для модели. </returns>
        public bool TryGetViews<TModel>(TModel model, out List<IAutoView> views) where TModel : IModel
        {
            var modelType = model.GetType();
            views = GetViews(modelType);

            return views.Count > 0;
        }

        /// <summary>
        /// Добавить представление.
        /// </summary>
        /// <param name="autoView"> Представление. </param>
        private void AddView(IAutoView autoView)
        {
            var modelType = autoView.ModelType;
            var viewTypeName = autoView.GetType().Name;

            if (modelType?.GetInterface(nameof(IModel))?.Name != nameof(IModel))
            {
                GameLogger.Error($"В представлении {viewTypeName}, не указан тип модели");
                return;
            }

            if (!_dictionary.ContainsKey(modelType))
            {
                GameLogger.Error($"Не было добавлено модели для представления {viewTypeName}");
                return;
            }

            var views = GetViews(modelType);
            views.Add(autoView);

            InitializeView(autoView);
        }

        /// <summary>
        /// Проверяет все представления сцены, предыдущей сцены.
        /// </summary>
        /// <remarks>
        /// Для случаев, когда существует синглтон живущий "игру",
        /// но связанные представления могут столько не жить.
        /// </remarks>
        private void CheckPreviousSceneViews()
        {
            foreach (var autoViewModelLink in _dictionary.Values)
            {
                var destroyedViews = autoViewModelLink.Views
                    .Where(v => MonoUtils.IsDestroyed(v.GameObject))
                    .ToList();

                destroyedViews.ForEach(v => autoViewModelLink.Views.Remove(v));
            }
        }

        /// <summary>
        /// Получить представления для модели.
        /// </summary>
        /// <param name="modelType"> Тип модели. </param>
        /// <returns> Представления. </returns>
        private List<IAutoView> GetViews(Type modelType)
        {
            return _dictionary.TryGetValue(modelType, out var autoViewModelLink)
                ? autoViewModelLink.Views
                : throw new Exception(
                    "Получение представления для модели должно происходить только после инициализации модели");
        }

        /// <summary>
        /// Инициализация представления.
        /// </summary>
        /// <param name="autoView"> Представление. </param>
        /// <returns> Модель на которую было привязано представление. </returns>
        private void InitializeView(IAutoView autoView)
        {
            var modelType = autoView.ModelType;
            if (!_dictionary.TryGetValue(modelType, out var autoViewModelLink))
            {
                GameLogger.Error($"Не удалось установить модель {modelType.Name} " +
                                 $"для представления {autoView.GetType().Name}.");
                return;
            }

            autoView.Initialize();

            if (autoView.IsVisible)
                autoView.UpdateView(autoViewModelLink.Model);
        }
    }
}