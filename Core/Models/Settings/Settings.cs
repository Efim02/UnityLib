namespace UnityLib.Core.Models.Settings
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using UnityLib.Architecture.MVC;
    using UnityLib.Core.Constants;
    using UnityLib.Core.Utils;

    /// <summary>
    /// Настройки в приложении.
    /// </summary>
    /// <remarks> Хранятся на клиенте. </remarks>
    public class Settings : SingleModel
    {
        /// <summary>
        /// Языки, и их подпись.
        /// </summary>
        public readonly Dictionary<SystemLanguage, string> Languages = new Dictionary<SystemLanguage, string>
        {
            { SystemLanguage.Russian, "Русский" },
            { SystemLanguage.English, "English" }
        };

        /// <summary>
        /// Путь к настройкам.
        /// </summary>
        private readonly string _pathSettings;

        /// <inheritdoc cref="AudioVolume" />
        private BindingProperty<float> _audioVolume;

        /// <inheritdoc cref="ReplicaVolume" />
        private BindingProperty<float> _replicaVolume;

        /// <inheritdoc cref="SoundVolume" />
        private BindingProperty<float> _soundVolume;

        /// <inheritdoc />
        public Settings()
        {
            _pathSettings = $"{Application.persistentDataPath}/{PathConstants.SETTINGS}";
            Load();
        }

        /// <summary>
        /// Громкость аудио треков.
        /// </summary>
        public BindingProperty<float> AudioVolume => _audioVolume ??= new BindingProperty<float>();

        /// <summary>
        /// Параметры команты, при ее создании.
        /// </summary>
        public RoomParameters CreateRoomParameters { get; set; }

        /// <summary>
        /// Тренирован ли игрок.
        /// </summary>
        public bool IsTrained { get; set; }

        /// <summary>
        /// Последнее время отправки отчета.
        /// </summary>
        public DateTime LastTimeReportSending { get; set; }

        /// <summary>
        /// Система частиц включена.
        /// </summary>
        public bool ParticleSystemEnabled { get; set; }

        /// <summary>
        /// Громкость реплик.
        /// </summary>
        public BindingProperty<float> ReplicaVolume => _replicaVolume ??= new BindingProperty<float>();

        /// <summary>
        /// Громкость звуков.
        /// </summary>
        public BindingProperty<float> SoundVolume => _soundVolume ??= new BindingProperty<float>();

        /// <summary>
        /// Выбранная локализация.
        /// </summary>
        public SystemLanguage SystemLanguage { get; set; }

        /// <summary>
        /// Сохранить настройки.
        /// </summary>
        public void Save()
        {
            var settingsDto = new SettingsDto();
            SettingsMapper.MapToDto(this, settingsDto);

            XmlUtils.Serialize(settingsDto, _pathSettings);
        }

        /// <summary>
        /// Загрузить настройки.
        /// </summary>
        private void Load()
        {
            var settingsDto = XmlUtils.Deserialize<SettingsDto>(_pathSettings) ?? new SettingsDto
            {
                AudioVolume = 0.24f,
                SoundVolume = 0.57f,
                SystemLanguage = Application.systemLanguage,
                ParticleSystemEnabled = true
            };

            SettingsMapper.MapToModel(settingsDto, this);

            UpdateView();
        }
    }
}