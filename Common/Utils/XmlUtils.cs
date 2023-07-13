namespace UnityLib.Common.Utils
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    using UnityLib.Common.GO.Logger;

    public static class XmlUtils
    {
        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="path"> Путь. </param>
        /// <returns> Объект. Null если не получилось десериализовать или нет файла. </returns>
        public static T Deserialize<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                GameLogger.Warning($"Xml сериализатор. Нет файла: {Path.GetFileName(path)}");
                return null;
            }

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using var stream = File.Open(path, FileMode.Open);

                return (T)xmlSerializer.Deserialize(stream);
            }
            catch (Exception exception)
            {
                GameLogger.Warning($"Не удалось получить объект {typeof(T)} из xml файла." +
                                   $"\nСообщение: {exception.Message}.");
                return null;
            }
        }

        /// <summary>
        /// Десериализовать xml разметку.
        /// </summary>
        /// <param name="xmlMarkup"> Xml разметка. </param>
        /// <returns> Объект. Null если не получилось десериализовать или нет файла. </returns>
        public static T DeserializeXml<T>(string xmlMarkup) where T : class
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlMarkup));

                return (T)xmlSerializer.Deserialize(stream);
            }
            catch (Exception exception)
            {
                GameLogger.Warning($"Не удалось получить объект {typeof(T)} из xml файла." +
                                   $"\nСообщение: {exception.Message}.");
                return null;
            }
        }

        /// <summary>
        /// Сериализовать.
        /// </summary>
        /// <param name="obj"> Объект. </param>
        /// <param name="path"> Путь к файлу. </param>
        public static void Serialize<T>(T obj, string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stream = File.Create(path);
            xmlSerializer.Serialize(stream, obj);
        }

        /// <summary>
        /// Сериализовать xml разметку.
        /// </summary>
        /// <param name="obj"> Объект. </param>
        /// <returns> Xml разметка. </returns>
        public static string SerializeXml<T>(T obj)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using var stream = new MemoryStream();
                xmlSerializer.Serialize(stream, obj);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception exception)
            {
                GameLogger.Warning($"Не удалось получить xml {typeof(T)} из объекта." +
                                   $"\nСообщение: {exception.Message}.");
                return null;
            }
        }
    }
}