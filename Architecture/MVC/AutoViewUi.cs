namespace UnityLib.Architecture.MVC
{
    using System;

    /// <summary>
    /// Абстракция одного из представлений для модели, которая автоматически подключается к модели.
    /// </summary>
    /// <typeparam name="TModel"> Модель. </typeparam>
    public abstract class AutoViewUi<TModel> : ViewUi<TModel>, IAutoView
        where TModel : SingleModel
    {
        /// <summary>
        /// Удаление представления.
        /// </summary>
        /// <remarks>Переопределить для обработки callback-а "Уничтожения".</remarks>
        public virtual void Destruct()
        {
        }

        /// <summary>
        /// Создание объекта, и добавление представления, для модели.
        /// </summary>
        /// <remarks>Переопределить для обработки callback-а "Инициализации".</remarks>
        public virtual void Initialize()
        {
        }

        /// <inheritdoc />
        public Type ModelType => typeof(TModel);
    }
}