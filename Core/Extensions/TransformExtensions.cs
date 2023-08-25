namespace UnityLib.Core.Extensions
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// Расширение для Transform.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Получает всех наследников у текущего объекта, и рекурсивно у всех его наследников.
        /// </summary>
        /// <param name="transform"> Трансформ. </param>
        /// <returns> Список всех объектов (имеющихся в иерархии указанного объекта) начиная с запрошенного. </returns>
        /// <remarks> Сам указанный объект тоже добавляется в список. </remarks>
        public static List<Transform> GetAllFromHierarchy(this Transform transform)
        {
            var transforms = new List<Transform> { transform };
            transforms.AddRange(transform.GetChildrenRecursive());

            return transforms;
        }

        /// <summary>
        /// Получить потомков.
        /// </summary>
        /// <returns> Список потомков, на один уровень. </returns>
        public static List<Transform> GetChildren(this Transform transform)
        {
            var children = new List<Transform>();
            for (var index = 0; index < transform.childCount; index++)
            {
                var child = transform.GetChild(index);
                children.Add(child);
            }

            return children;
        }

        /// <summary>
        /// Получает всех наследников у текущего объекта, и рекурсивно у всех его наследников.
        /// </summary>
        /// <param name="transform"> Трансформ. </param>
        /// <returns> Список всех объектов (имеющихся в иерархии указанного объекта) начиная с запрошенного. </returns>
        private static IEnumerable<Transform> GetChildrenRecursive(this Transform transform)
        {
            var children = transform.GetChildren();
            var transforms = new List<Transform>();

            foreach (var child in children)
            {
                transforms.Add(child);
                transforms.AddRange(child.GetChildrenRecursive());
            }

            return transforms;
        }
    }
}