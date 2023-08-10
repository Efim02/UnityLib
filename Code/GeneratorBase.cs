namespace UnityLib.Code
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Базовый класс генератора.
    /// </summary>
    public abstract class GeneratorBase
    {
        /// <summary>
        /// Название сборки, с типами которой работаем.
        /// </summary>
        public abstract string AssemblyName { get; }

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

        /// <summary>
        /// Получить сборку по <see cref="AssemblyName" />.
        /// </summary>
        /// <returns> Сборка. </returns>
        protected Assembly GetAssembly()
        {
            return AppDomain.CurrentDomain.GetAssemblies().First(a => a.ManifestModule.ScopeName == AssemblyName);
        }
    }
}