namespace UnityLib.Core.Helpers.Json
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    /// Помощник с работой в Unity json сериализаторе, при нескольких объектах.
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// Получить объекты из Json файла.
        /// </summary>
        /// <typeparam name="T"> Тип объекта. </typeparam>
        /// <param name="jsonText"> Текст со списком объектов. </param>
        /// <returns> Объекты. </returns>
        public static IEnumerable<T> ListFromJson<T>(string jsonText)
        {
            var jsonFieldOfObjects = JsonUtility.FromJson<JsonArray<T>>(jsonText);
            return jsonFieldOfObjects.Items;
        }

        /// <summary>
        /// В json файл.
        /// </summary>
        /// <typeparam name="T"> Тип объекта. </typeparam>
        /// <param name="enumerable"> Перечисление объектов. </param>
        /// <param name="prettyPrint"> Читабельность. </param>
        /// <returns> Текст. </returns>
        public static string ListToJson<T>(IEnumerable<T> enumerable, bool prettyPrint)
        {
            var jsonObject = new JsonArray<T>
            {
                Items = enumerable.ToArray()
            };

            return JsonUtility.ToJson(jsonObject, prettyPrint);
        }
    }
}