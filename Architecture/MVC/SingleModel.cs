namespace UnityLib.Architecture.MVC
{
    using System;

    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Модель для синглтонов, к которой могут быть подключены представления.
    /// </summary>
    public abstract class SingleModel : BaseModel, IDisposable
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SingleModel()
        {
            AutoViewModelConnector.AddModel(this);
        }

        /// <summary>
        /// Уничтожить объект, чтобы создать новые его копии.
        /// </summary>
        public void Dispose()
        {
            AutoViewModelConnector.RemoveModel(this);
            Destroy();
        }

        /// <inheritdoc />
        public override void SetVisibleView(bool visible)
        {
            if (AutoViewModelConnector.TryGetViews(this, out var views))
            {
                DispatcherUtils.SafeInvoke(() => views.ForEach(view =>
                {
                    // Сначала обновить данные представления, потом отобразить.
                    view.UpdateView(this);
                    view.IsVisible = visible;
                }));
            }
        }

        /// <summary>
        /// Обновить представление или представления.
        /// </summary>
        public override void UpdateView()
        {
            if (AutoViewModelConnector.TryGetViews(this, out var views))
                DispatcherUtils.SafeInvoke(() => views.ForEach(view => view.UpdateView(this)));
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