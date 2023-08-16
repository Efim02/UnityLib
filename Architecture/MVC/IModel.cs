namespace UnityLib.Architecture.MVC
{
    /// <summary>
    /// Интерфейс для модели.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Установить видимость представлениям.
        /// </summary>
        /// <param name="visible"> Видимость. </param>
        void SetVisibleView(bool visible);

        /// <summary>
        /// Обновить представление.
        /// </summary>
        /// <remarks> Представление станет видимым. </remarks>
        void UpdateView();
    }
}