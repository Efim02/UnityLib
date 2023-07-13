namespace UnityLib.MVC
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// Модель с реализацией <see cref="MonoBehaviour" /> и представлением, привязанным через Inspector.
    /// </summary>
    public class MonoModel : MonoBehaviour, IModel
    {
        /// <summary>
        /// Представление. Указывается в Ui.
        /// </summary>
        public List<IView> Views;

        /// <inheritdoc />
        public void SetVisibleView(bool visible)
        {
            foreach (var view in Views)
            {
                view.IsVisible = visible;
            }
        }

        /// <summary>
        /// Обновить представление или представления.
        /// </summary>
        public void UpdateView()
        {
            foreach (var view in Views)
            {
                view.UpdateView(this);
                view.IsVisible = true;
            }
        }
    }
}