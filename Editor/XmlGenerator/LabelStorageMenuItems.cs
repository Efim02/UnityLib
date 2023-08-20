namespace UnityLib.Editor.XmlGenerator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Core.Constants;
    using UnityLib.Core.Extensions;
    using UnityLib.Core.Models.Localization;
    using UnityLib.Core.Utils;

    public class LabelStorageMenuItems
    {
        /// <summary>
        /// Ресурсы.
        /// </summary>
        private static readonly string _localization = Path.Combine(
            Application.dataPath,
            PathConstants.RESOURCES_FOLDER,
            Localizer.LOCALIZATION_FOLDER);

        /// <summary>
        /// Построить json, пример заполнения.
        /// </summary>
        [MenuItem("Игра/Генераторы Xml/Локализация/Создать файлы")]
        public static void BuildJsonCommand()
        {
            var fileNames = GetFileNames();

            Directory.CreateDirectory(_localization);
            foreach (var fileName in fileNames)
            {
                var path = Path.Combine(_localization, fileName);
                if (File.Exists(path))
                {
                    GameLogger.Error($"Файл {fileName} с надписями, существует!\n{path}");
                    continue;
                }

                XmlUtils.Serialize(new LabelStorageDto(new List<LabelDto> { new() }), path);
            }
        }

        /// <summary>
        /// Проверить количество, команда.
        /// </summary>
        [MenuItem("Игра/Генераторы Xml/Локализация/Проверить файлы")]
        public static void CheckCountCommand()
        {
            GetFileNames().ForEach(CheckFile);
        }

        /// <summary>
        /// Проверить количество.
        /// </summary>
        private static void CheckFile(string fileName)
        {
            // INFUT: Добавть поиск по файлам кода
            var editorKeyLabels = SceneUtils.GetAllScenes()
                .SelectMany(scene => scene.GetRootGameObjects())
                .Select(go => go.transform)
                .SelectMany(TransformExtensions.GetAllFromHierarchy)
                .Select(go => go.GetComponent<LocalizationComponent>())
                .Where(go => go != null)
                .Select(lc => lc.LabelKey)
                .ToList();

            var path = Path.Combine(_localization, fileName);
            if (!File.Exists(path))
            {
                GameLogger.Warning($"Отсутствует файл -{path}-");
                return;
            }

            GameLogger.Info($"Проверка файла надписей - {fileName}");

            var labelStorageDto = XmlUtils.Deserialize<LabelStorageDto>(path);
            if (editorKeyLabels.Count < labelStorageDto.Labels.Count)
            {
                GameLogger.Error("Количество надписей, не совпадает, с количеством в перечислении.");
                return;
            }

            var notContainedKey = new List<string>();
            foreach (var editorKey in editorKeyLabels)
            {
                if (labelStorageDto.Labels.All(i => i.Key != editorKey))
                    notContainedKey.Add(editorKey);
            }

            if (notContainedKey.Any())
            {
                GameLogger.Error(
                    $"Нет совпадения в надписях. В словарь добавлены надписи:\n{notContainedKey.ToText()}");
                return;
            }

            GameLogger.Info("Списки надписей валидны.");
        }

        /// <summary>
        /// Получить путь к Json документу.
        /// </summary>
        private static List<string> GetFileNames()
        {
            return new List<string>
            {
                Localizer.LABELS_EN + PathConstants.XML,
                Localizer.LABELS_RU + PathConstants.XML
            };
        }
    }
}