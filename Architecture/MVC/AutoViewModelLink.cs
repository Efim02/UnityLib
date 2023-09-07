namespace UnityLib.Architecture.MVC
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Связь между моделью и представлениями.
    /// </summary>
    internal class AutoViewModelLink
    {
        /// <summary>
        /// Связь между моделью и представлениями.
        /// </summary>
        /// <param name="modelType"> Тип модели. </param>
        /// <param name="model"> Модель. </param>
        public AutoViewModelLink(Type modelType, SingleModel model)
        {
            ModelType = modelType;
            Model = model;
            Views = new List<IAutoView>();
        }

        /// <summary>
        /// Модель.
        /// </summary>
        public SingleModel Model { get; set; }

        /// <summary>
        /// Тип модели.
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// Представления.
        /// </summary>
        public List<IAutoView> Views { get; set; }
    }
}