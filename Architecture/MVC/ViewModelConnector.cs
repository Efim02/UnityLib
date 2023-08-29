namespace UnityLib.Architecture.MVC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Связыватель конкретной одной модели и ее представлений.
    /// </summary>
    public static class ViewModelConnector
    {
        /// <summary>
        /// Словарь со списками моделей; используем для инициализации представлений.
        /// </summary>
        private static readonly Dictionary<Type, SingleModel> _modelsDictionary;

        /// <summary>
        /// Словарь с представлениями для вида.
        /// </summary>
        private static readonly Dictionary<Type, List<IView>> _viewsByModel;

        /// <summary>
        /// Связыватель конкретной одной модели и ее представлений.
        /// </summary>
        static ViewModelConnector()
        {
            _viewsByModel = new Dictionary<Type, List<IView>>();
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
        /// Добавить представление.
        /// </summary>
        /// <param name="view"> Представление. </param>
        public static void AddView<TModel>(IView view) where TModel : IModel
        {
            var views = GetViews(typeof(TModel));
            if (views.Contains(view) && views.Count > 1)
            {
                GameLogger.Warning($"Это представление уже было добавлено {typeof(TModel)}." +
                                   $"\nКоличество: {views.Count}, Существует: " +
                                   $"{views.Cast<MonoBehaviour>().Count(mn => !MonoUtils.IsDestroyed(mn))}");
                return;
            }

            views.Add(view);
        }

        /// <summary>
        /// Очистить словарь сцены, с моделями и представлениями живущими одну сцену.
        /// </summary>
        public static void ClearSceneDictionary()
        {
            var modelsDictionary = _modelsDictionary.ToArray();
            foreach (var pairModel in modelsDictionary)
            {
                if (pairModel.Value.IsSceneModel)
                    _modelsDictionary.Remove(pairModel.Key);
            }
        }

        /// <summary>
        /// Инициализация представления.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="view"> Представление. </param>
        /// <returns> Модель на которую было привязано представление. </returns>
        public static TModel InitializeView<TModel>(IView view) where TModel : SingleModel
        {
            if (!_modelsDictionary.TryGetValue(typeof(TModel), out var model))
            {
                GameLogger.Error($"Не удалось установить модель {typeof(TModel).Name} " +
                                 $"для представления {view.GetType().Name}.");
                return null;
            }

            view.IsVisible = true;
            view.UpdateView(model);

            return (TModel)model;
        }

        /// <summary>
        /// Удалить модель.
        /// </summary>
        /// <typeparam name="TModel"> Тип модели. </typeparam>
        /// <param name="model"> Модель. </param>
        public static void RemoveModel<TModel>(TModel model) where TModel : SingleModel
        {
            var modelType = model.GetType();
            _modelsDictionary.Remove(modelType);
        }

        /// <summary>
        /// Удалить представление.
        /// </summary>
        /// <param name="view"> Представление. </param>
        public static void RemoveView<TModel>(IView view) where TModel : IModel
        {
            var views = GetViews(typeof(TModel));
            views.Remove(view);
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
        /// Получить представления для модели.
        /// </summary>
        /// <param name="modelType"> Тип модели. </param>
        /// <returns> Представления. </returns>
        private static List<IView> GetViews(Type modelType)
        {
            if (_viewsByModel.TryGetValue(modelType, out var views))
                return views;

            views = new List<IView>();
            _viewsByModel.Add(modelType, views);

            return views;
        }
    }
}