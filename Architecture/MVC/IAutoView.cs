namespace UnityLib.Architecture.MVC
{
    using System;

    /// <summary>
    /// Представление для модели, которое автоматически подключается к модели.
    /// </summary>
    /// <remarks> Нужно для поиска по компонентам. </remarks>
    public interface IAutoView : IView
    {
        /// <summary>
        /// Тип модели, к которому привязали представление.
        /// </summary>
        /// <remarks> По типу происходит поиск всех представлений на сцене. </remarks>
        Type ModelType { get; }

        /// <summary>
        /// Удаление представления.
        /// </summary>
        void Destruct();

        /// <summary>
        /// Создание объекта, и добавление представления, для модели.
        /// </summary>
        void Initialize();
    }
}