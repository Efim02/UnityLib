namespace UnityLib.Architecture.MVC
{
    using System;

    /// <summary>
    /// Модель для синглтонов.
    /// </summary>
    public class SingleModel : IModel, IDisposable
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SingleModel()
        {
            ViewModelConnector.AddModel(this);
        }

        /// <summary>
        /// Конструктор модели.
        /// </summary>
        /// <param name="view">
        /// Представление.
        /// Укажите Null если привязка для единственного экземпляра.
        /// </param>
        public SingleModel(IView view) : this()
        {
            View = view;
        }

        /// <summary>
        /// Это модель одной сцены.
        /// </summary>
        public virtual bool IsSceneModel => false;

        /// <summary>
        /// Представление.
        /// </summary>
        public IView View { get; }

        /// <summary>
        /// Уничтожить объект, чтобы создать новые его копии.
        /// </summary>
        public void Dispose()
        {
            Destroy();
            ViewModelConnector.RemoveModel(this);
        }

        /// <inheritdoc />
        public void SetVisibleView(bool visible)
        {
            if (View != null)
                View.IsVisible = visible;

            if (!ViewModelConnector.TryGetViews(this, out var views))
                return;

            foreach (var view in views)
            {
                view.IsVisible = visible;
            }
        }

        /// <summary>
        /// Обновить представление или представления.
        /// </summary>
        public void UpdateView()
        {
            if (View != null)
            {
                View.UpdateView(this);
                return;
            }

            if (!ViewModelConnector.TryGetViews(this, out var views))
                return;

            foreach (var view in views)
            {
                view.UpdateView(this);
            }
        }

        /// <summary>
        /// Уничтожить модель.
        /// </summary>
        /// <remarks> Реализовать, если необходим такой callback. </remarks>
        protected virtual void Destroy()
        {
        }
    }
}