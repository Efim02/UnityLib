namespace UnityLib.Editor.CodeGenerator
{
    using System;

    /// <summary>
    /// Базовый класс генератора.
    /// </summary>
    public abstract class GeneratorBase
    {
        /// <summary>
        /// Данные закешированы ли.
        /// </summary>
        public abstract bool IsCached { get; set; }

        /// <summary>
        /// Генерировать.
        /// </summary>
        public abstract void Generate();

        /// <summary>
        /// Имеются ли изменения, по которым нужно сгенерировать код.
        /// </summary>
        /// <returns> TRUE если да. </returns>
        public abstract bool HasChanges();

        /// <summary>
        /// Начать генерацию.
        /// </summary>
        public void StartGenerate()
        {
            if (!IsCached)
                throw new Exception("Генератор не проверил изменения.");
            Generate();
        }
    }
}