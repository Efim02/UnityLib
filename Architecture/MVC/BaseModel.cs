namespace UnityLib.Architecture.MVC
{
    using System;

    /// <summary>
    /// Базовая модель.
    /// </summary>
    public abstract class BaseModel : IModel
    {
        /// <inheritdoc />
        public abstract void SetVisibleView(bool visible);

        /// <inheritdoc />
        public abstract void UpdateView();

        /// <summary>
        /// Сделает видимым и обновит представление.
        /// </summary>
        public void VisibleAndUpdateView()
        {
            UpdateView();
            SetVisibleView(true);
        }
    }
}