namespace UnityLib.Architecture.MVC
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Абстракция одного из представлений для модели, которая автоматически подключается к модели.
    /// </summary>
    /// <typeparam name="TModel"> Модель. </typeparam>
    public abstract class AutoViewUi<TModel> : ViewUi<TModel>, IAutoView
        where TModel : SingleModel
    {
        /// <inheritdoc />
        /// <remarks> Переопределить для обработки callback-а "Уничтожения". </remarks>
        public virtual void Destruct()
        {
        }

        /// <inheritdoc />
        /// <remarks> Переопределить для обработки callback-а "Инициализации". </remarks>
        public virtual void Initialize()
        {
        }

        /// <inheritdoc />
        public GameObject GameObject => gameObject;

        /// <inheritdoc />
        public Type ModelType => typeof(TModel);
    }
}