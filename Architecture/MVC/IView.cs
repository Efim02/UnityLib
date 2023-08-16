namespace UnityLib.Architecture.MVC
{
    /// <summary>
    /// Интерфейс для представления.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Включено.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Инициализация представления, через модель.
        /// </summary>
        /// <param name="model"> Модель. </param>
        void InitializeView(IModel model);

        /// <summary>
        /// Инициализация представления.
        /// </summary>
        void InitializeView();

        /// <summary>
        /// Обновить представление.
        /// </summary>
        /// <param name="model"> Модель. </param>
        void UpdateView(IModel model);
    }
}