namespace UnityLib.Architecture.MVC
{
    using System;
    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Модель для синглтонов, к которой могут быть подключены представления.
    /// </summary>
    public abstract class SingleModel : BaseModel, IDisposable
    {
        /// <summary>
        /// Уничтожена ли модель.
        /// </summary>
        public bool IsDestroyed { get; private set; }
        
        private AutoViewModelLinker AutoViewModelLinker => Injector.Get<AutoViewModelLinker>();

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SingleModel()
        {
            AutoViewModelLinker.AddModel(this);
        }

        /// <summary>
        /// Уничтожить объект, чтобы создать новые его копии.
        /// </summary>
        public void Dispose()
        {
            AutoViewModelLinker.RemoveModel(this);
            Destroy();
            IsDestroyed = true;
        }

        /// <inheritdoc />
        public override void SetVisibleView(bool visible)
        {
            if (IsDestroyed)
                return;

            if (AutoViewModelLinker.TryGetViews(this, out var views))
            {
                DispatcherUtils.SafeInvoke(() => views.ForEach(view =>
                {
                    // Сначала обновить данные представления, потом отобразить.
                    if (visible)
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
            if (IsDestroyed)
                return;
            
            if (AutoViewModelLinker.TryGetViews(this, out var views))
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