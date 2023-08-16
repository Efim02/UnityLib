namespace UnityLib.Architecture.Di
{
    using System;
    using System.Linq;

    /// <summary>
    /// Требовательный инстанс, у которого объект имеет конструктор с параметрами.
    /// </summary>
    internal class PickyInstance
    {
        /// <summary>
        /// Требовательный инстанс, у которого объект имеет конструктор с параметрами.
        /// </summary>
        /// <param name="existsOnScene"> Объект живет одну сцену. </param>
        /// <param name="interfaceType"> Тип интерфейса. </param>
        /// <param name="type"> Тип объекта. </param>
        /// <param name="parameters"> Параметры конструктора объекта. </param>
        internal PickyInstance(bool existsOnScene, Type interfaceType, Type type, Type[] parameters)
        {
            ExistsOnScene = existsOnScene;
            InterfaceType = interfaceType;
            Parameters = parameters;
            Type = type;
        }

        /// <summary>
        /// Объект живет одну сцену.
        /// </summary>
        internal bool ExistsOnScene { get; }

        /// <summary>
        /// Тип интерфейса.
        /// </summary>
        internal Type InterfaceType { get; }

        /// <summary>
        /// Параметры конструктора объекта.
        /// </summary>
        /// <remarks>
        /// Хранятся в правильном порядке.
        /// </remarks>
        internal Type[] Parameters { get; }

        /// <summary>
        /// Тип объекта.
        /// </summary>
        internal Type Type { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Type.Name} ({Parameters.Select(p => p.Name).Aggregate((f, l) => $"{f}, {l}")})";
        }
    }
}