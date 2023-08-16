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
    }
}