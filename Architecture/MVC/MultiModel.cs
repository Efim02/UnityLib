namespace UnityLib.Architecture.MVC
{
    using System.Collections.Generic;

    /// <summary>
    /// Модель, которая существует не единственном экземпляре.
    /// </summary>
    public abstract class MultiModel : BaseModel
    {
        /// <summary>
        /// Модель, которая существует не единственном экземпляре.
        /// </summary>
        public MultiModel()
        {
            Views = new List<IView>();
        }

        /// <summary>
        /// Представление.
        /// </summary>
        public List<IView> Views { get; }

        /// <inheritdoc />
        public override void SetVisibleView(bool visible)
        {
            Views.ForEach(v => v.IsVisible = visible);
        }

        /// <inheritdoc />
        public override void UpdateView()
        {
            Views.ForEach(v => v.UpdateView(this));
        }
    }
}