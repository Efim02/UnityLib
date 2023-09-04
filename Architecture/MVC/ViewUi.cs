namespace UnityLib.Architecture.MVC
{
    using System;

    using UnityEngine;

    using UnityLib.Architecture.Log;

    /// <summary>
    /// Абстракция представления для модели.
    /// </summary>
    /// <typeparam name="TModel"> Тип модели. </typeparam>
    public abstract class ViewUi<TModel> : MonoBehaviour, IView
        where TModel : IModel
    {
        /// <summary>
        /// Включено.
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                // Отлавливаем ошибки, чтобы не отвалился слой BL из-за UI.
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
        /// Обновить представление.
        /// </summary>
        /// <param name="model"> Типизированная модель. </param>
        protected abstract void UpdateView(TModel model);
    }
}