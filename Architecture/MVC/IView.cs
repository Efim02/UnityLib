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
        /// Обновить представление.
        /// </summary>
        /// <param name="model"> Модель. </param>
        void UpdateView(IModel model);
    }
}