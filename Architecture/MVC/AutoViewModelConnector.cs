namespace UnityLib.Architecture.MVC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Utils;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Связыватель конкретной одной модели и ее представлений.
    /// </summary>
    internal static class AutoViewModelConnector
    {
        // TODO: Соединить словари.

        /// <summary>
        /// Словарь со списками моделей; используем для инициализации представлений.
        /// </summary>
        private static readonly Dictionary<Type, SingleModel> _modelsDictionary;

        /// <summary>
        /// Словарь с представлениями для вида.
        /// </summary>
        private static readonly Dictionary<Type, List<IAutoView>> _viewsByModel;
        
        // TODO: Добавить вызовы внутри SceneChanger

        /// <summary>
        /// Связыватель конкретной одной модели и ее представлений.
        /// </summary>
        static AutoViewModelConnector()
        {
            _viewsByModel = new Dictionary<Type, List<IAutoView>>();
            _modelsDictionary = new Dictionary<Type, SingleModel>();
        }

        /// <summary>
        /// Добавить модель.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="model"> Модель. </param>
        public static void AddModel<TModel>(TModel model) where TModel : SingleModel
        {
            var modelType = model.GetType();
            if (_modelsDictionary.ContainsKey(modelType))
                GameLogger.Warning($"Модель {modelType.Name} уже зарегистрирована; синглтон.");

            _modelsDictionary.Add(modelType, model);
        }

        /// <summary>
        /// Добавляет все представления сцены.
        /// </summary>
        public static void AddSceneViews()
        {
            var allObjects = Object.FindObjectsOfType<GameObject>();
            var autoViews = allObjects.SelectMany(o => o.GetComponents<IAutoView>()).ToList();

            autoViews.ForEach(AddView);
        }
        
        /// <summary>
        /// Удалить модель.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="model"> Модель. </param>
        public static void RemoveModel<TModel>(TModel model) where TModel : SingleModel
        {
            // TODO: Удаляем ссылки на представления и сами игровые объекты представлений.
            var modelType = model.GetType();
            _modelsDictionary.Remove(modelType);
        }
        
        /// <summary>
        /// Получить представления, для модели.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="views"> Представления. </param>
        /// <param name="model"> Модель. </param>
        /// <returns> TRUE - если есть представления для модели. </returns>
        public static bool TryGetViews<TModel>(TModel model, out List<IView> views) where TModel : IModel
        {
            var modelType = model.GetType();
            views = GetViews(modelType);

            return views.Count > 0;
        }

        /// <summary>
        /// Добавить представление.
        /// </summary>
        /// <param name="autoView"> Представление. </param>
        private static void AddView(IAutoView autoView)
        {
            var modelType = autoView.ModelType;
            var viewTypeName = autoView.GetType().Name;

            if (modelType?.GetInterface(nameof(IModel))?.Name != nameof(IModel))
            {
                GameLogger.Error($"В представлении {viewTypeName}, не указан тип модели");
                return;
            }
            
            if (!_modelsDictionary.ContainsKey(modelType))
            {
                GameLogger.Error($"Не было добавлено модели для представления {viewTypeName}");
                return;
            }

            var views = GetViews(modelType);
            views.Add(autoView);

            InitializeView(autoView);
        }

        /// <summary>
        /// Получить представления для модели.
        /// </summary>
        /// <param name="modelType"> Тип модели. </param>
        /// <returns> Представления. </returns>
        private static List<IAutoView> GetViews(Type modelType)
        {
            if (_viewsByModel.TryGetValue(modelType, out var views))
                return views;

            views = new List<IAutoView>();
            _viewsByModel.Add(modelType, views);

            return views;
        }

        /// <summary>
        /// Инициализация представления.
        /// </summary>
        /// <param name="autoView"> Представление. </param>
        /// <returns> Модель на которую было привязано представление. </returns>
        private static void InitializeView(IAutoView autoView)
        {
            var modelType = autoView.ModelType;
            if (!_modelsDictionary.TryGetValue(modelType, out var model))
            {
                GameLogger.Error($"Не удалось установить модель {modelType.Name} " +
                                 $"для представления {autoView.GetType().Name}.");
                return;
            }

            autoView.Initialize();

            if (autoView.IsVisible)
                autoView.UpdateView(model);
        }

        /// <summary>
        /// Удалить представление.
        /// </summary>
        /// <param name="autoView"> Представление. </param>
        private static void RemoveView<TModel>(IAutoView autoView) where TModel : IModel
        {
            var views = GetViews(typeof(TModel));
            views.Remove(autoView);
        }
    }
}