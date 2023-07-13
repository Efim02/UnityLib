namespace UnityLib.MVC
{
    using System;

    using UnityEngine;

    using UnityLib.Common;
    using UnityLib.Common.GO.Logger;

    /// <summary>
    /// Абстракция представления для модели.
    /// </summary>
    /// <typeparam name="TModel"> Тип модели. </typeparam>
    public abstract class ViewUi<TModel> : MonoBehaviour, IView
        where TModel : IModel
    {
        /// <inheritdoc />
        /// <remarks> Реализовать, если необходим такой callback. </remarks>
        public virtual void InitializeView()
        {
        }


        /// <inheritdoc />
        public void InitializeView(IModel model)
        {
            if (model is not TModel tModel)
            {
                GameLogger.Error($"Ошибка привязки Model-View: {model.GetType().Name} " +
                                 $"не является {typeof(TModel).Name}.");
                return;
            }

            try
            {
                InitializeView(tModel);
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception, $"Ошибка инициализации представления - {GetType().Name}.");
            }
        }

        /// <summary>
        /// Включено.
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                try
                {
                    return gameObject.activeSelf;
                }
                catch (Exception exception)
                {
                    GameLogger.Error(exception, $"Ошибка получения видимости представления - {GetType().Name}.");
                    return true;
                }
            }
            set
            {
                try
                {
                    gameObject.SetActive(value);
                }
                catch (Exception exception)
                {
                    GameLogger.Error(exception, $"Ошибка изменения видимости представления - {GetType().Name}.");
                }
            }
        }

        /// <inheritdoc />
        public void UpdateView(IModel model)
        {
            if (model is not TModel tModel)
            {
                GameLogger.Error($"Ошибка привязки Model-View: {model.GetType().Name} " +
                                 $"не является {typeof(TModel).Name}.");
                return;
            }

            try
            {
                UpdateView(tModel);
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception, $"Ошибка обновления представления - {GetType().Name}.");
            }
        }

        /// <summary>
        /// Инициализация представления, через модель.
        /// </summary>
        /// <param name="model"> Модель. </param>
        /// <remarks> Реализовать, если необходим такой callback. </remarks>
        protected virtual void InitializeView(TModel model)
        {
        }

        /// <summary>
        /// Обновить представление.
        /// </summary>
        /// <param name="model"> Типизированная модель. </param>
        protected abstract void UpdateView(TModel model);
    }
}