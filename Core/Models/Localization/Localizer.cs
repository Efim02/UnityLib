namespace UnityLib.Core.Models.Localization
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Core.Models.Settings;
    using UnityLib.Core.Utils;

    /// <summary>
    /// Большой класс констант надписей.
    /// </summary>
    /// <remarks>
    /// <para> В дальнейшем будет использован в локализации. </para>
    /// Изначально словарь заполнен русскими словами.
    /// </remarks>
    public class Localizer
    {
        public const string LABELS_EN = "labels_en";
        public const string LABELS_RU = "labels_ru";
        public const string LOCALIZATION_FOLDER = "localization";

        /// <summary>
        /// Доступные локализации.
        /// </summary>
        private static readonly IReadOnlyDictionary<SystemLanguage, string> _availableLocalizations =
            new Dictionary<SystemLanguage, string>
            {
                { SystemLanguage.Russian, $@"{LOCALIZATION_FOLDER}/{LABELS_RU}" },
                { SystemLanguage.English, $@"{LOCALIZATION_FOLDER}/{LABELS_EN}" },
            };

        /// <summary>
        /// Активные надписи.
        /// </summary>
        private readonly List<ILabel> _activeLabels;

        /// <summary>
        /// Словарь.
        /// </summary>
        private Dictionary<string, string> _dictionary;

        /// <inheritdoc cref="Localizer" />
        public Localizer(Settings settings)
        {
            _dictionary = new Dictionary<string, string>();
            _activeLabels = new List<ILabel>();

            LoadLabels(settings.SystemLanguage);
        }

        /// <summary>
        /// Добавить сущность надписи.
        /// </summary>
        /// <param name="label"> Надпись. </param>
        public void AddLabel(ILabel label)
        {
            _activeLabels.Add(label);
        }

        /// <summary>
        /// Получить надпись по ключу.
        /// </summary>
        /// <param name="keyLabel"> Ключ к надписи. </param>
        /// <returns> Надпись. </returns>
        /// <remarks> На выбранном языке. </remarks>
        public string GetLabel(string keyLabel)
        {
            if (!_dictionary.ContainsKey(keyLabel))
            {
                GameLogger.Error($"Отсутствует перевод для \"{keyLabel}\"");
                return keyLabel;
            }

            return _dictionary[keyLabel];
        }

        /// <summary>
        /// Загрузить локализацию с надписями.
        /// </summary>
        public void LoadLabels(SystemLanguage language)
        {
            var availableLanguage = _availableLocalizations.Keys.Contains(language)
                ? language
                : SystemLanguage.English;
            var pathLocalization = _availableLocalizations[availableLanguage];

            GameLogger.Info($"Загружены надписи с языком \"{availableLanguage}\"");

            var xmlFile = Resources.Load<TextAsset>(pathLocalization);
            var labelStorageDto = XmlUtils.DeserializeXml<LabelStorageDto>(xmlFile.text);

            _dictionary = labelStorageDto.Labels.ToDictionary(l => l.Key, l => l.Label);

            // Обновление надписей.
            _activeLabels.ForEach(l => l.UpdateLabel());
        }

        /// <summary>
        /// Удалить сущность надписи.
        /// </summary>
        /// <param name="label"> Надпись. </param>
        public void RemoveLabel(ILabel label)
        {
            _activeLabels.Remove(label);
        }
    }
}