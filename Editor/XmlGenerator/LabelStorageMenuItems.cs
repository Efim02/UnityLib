namespace UnityLib.Editor.XmlGenerator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.SceneManagement;

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
            // INFUT: Добавить поиск по файлам кода
            var editorKeyLabels = new List<Scene> { SceneManager.GetActiveScene() }
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

            GameLogger.Info("Проверка файла надписей - ");

            var labelStorageDto = XmlUtils.Deserialize<LabelStorageDto>(path);
            var notContainedKey = new List<string>();

            var labels = labelStorageDto.Labels;
            foreach (var editorKey in editorKeyLabels)
            {
                if (labels.All(i => i.Key != editorKey))
                    notContainedKey.Add(editorKey);
            }

            // INFUT: Выводить или ссылки или адреса до объектов Unity, без ключа.
            if (notContainedKey.Any())
            {
                GameLogger.Error($"Словарь {fileName} не валиден, нет совпадения в надписях. " +
                                 $"В словарь добавлены надписи:\n{notContainedKey.ToText()}");
                labels.AddRange(notContainedKey.Select(k => new LabelDto { Key = k, Label = k }));
                XmlUtils.Serialize(labelStorageDto, path);
                return;
            }

            var notValidLabels = labels.Where(l => string.IsNullOrWhiteSpace(l.Label)).ToList();
            if (notValidLabels.Any())
            {
                GameLogger.Warning($"Словарь {fileName} имеет не заполненные значения:\n " +
                                   $"{notValidLabels.ToText(l => l.Key)}");
                return;
            }

            GameLogger.Info($"Словарь {fileName} валиден.");
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