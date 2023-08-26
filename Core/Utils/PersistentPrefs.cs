namespace UnityLib.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Core.Helpers.Json;

    /// <summary>
    /// Замена обычным PlayerPrefs, EditorPrefs, чтобы не засорять конфиг.
    /// </summary>
    public class PersistentPrefs
    {
        private static readonly string _prefsPath = Path.Combine(Application.persistentDataPath, "prefs.xml");

        public static T Get<T>(string key)
        {
            var keyValues = LoadKeyValues();
            var keyValue = keyValues.FirstOrDefault(k => k.Key == key);

            var value = keyValue?.Value ?? default(T)?.ToString();
            return value == null ? default : XmlUtils.DeserializeXml<T>(value);
        }

        public static void Set<T>(string key, T value)
        {
            var keyValues = LoadKeyValues();
            var keyValue = keyValues.FirstOrDefault(k => k.Key == key);

            if (keyValue == null)
                keyValues.Add(new KeyValue<string, string> { Key = key, Value = XmlUtils.SerializeXml(value) });
            else
                keyValue.Value = XmlUtils.SerializeXml(value);

            SaveKeyValues(keyValues);
        }

        private static List<KeyValue<string, string>> LoadKeyValues()
        {
            return XmlUtils.Deserialize<List<KeyValue<string, string>>>(_prefsPath)
                   ?? new List<KeyValue<string, string>>();
        }

        private static void SaveKeyValues(List<KeyValue<string, string>> keyValues)
        {
            XmlUtils.Serialize(keyValues.Distinct().ToList(), _prefsPath);
        }
    }
}