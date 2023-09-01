namespace UnityLib.Core.Models.Settings
{
    /// <summary>
    /// Маппер для настроек.
    /// </summary>
    public static class SettingsMapper
    {
        /// <summary>
        /// Обновить данные из модели.
        /// </summary>
        /// <param name="settings"> Настройки. </param>
        /// <param name="settingsDto"> Dto модель настроек. </param>
        public static void MapToDto(Settings settings, SettingsDto settingsDto)
        {
            settingsDto.IsTrained = settings.IsTrained;
            settingsDto.AudioVolume = settings.AudioVolume.Value;
            settingsDto.SoundVolume = settings.SoundVolume.Value;
            settingsDto.LastTimeReportSending = settings.LastTimeReportSending;
            settingsDto.SystemLanguage = settings.SystemLanguage;
            settingsDto.ParticleSystemEnabled = settings.ParticleSystemEnabled;
        }

        /// <summary>
        /// Обновить данные из Dto.
        /// </summary>
        /// <param name="settingsDto"> Настроек Dto. </param>
        /// <param name="settings"> Настройки. </param>
        public static void MapToModel(SettingsDto settingsDto, Settings settings)
        {
            settings.IsTrained = settingsDto.IsTrained;
            settings.SoundVolume.Value = settingsDto.SoundVolume;
            settings.AudioVolume.Value = settingsDto.AudioVolume;
            settings.LastTimeReportSending = settingsDto.LastTimeReportSending;
            settings.SystemLanguage = settingsDto.SystemLanguage;
            settings.ParticleSystemEnabled = settingsDto.ParticleSystemEnabled;
        }
    }
}