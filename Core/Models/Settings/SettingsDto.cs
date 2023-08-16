namespace UnityLib.Core.Models.Settings
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Dto модель настроек.
    /// </summary>
    public class SettingsDto
    {
        public SettingsDto()
        {
            CreateRoomParameters = new RoomParameters();
        }

        /// <summary>
        /// Громкость аудио.
        /// </summary>
        public float AudioVolume { get; set; }

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
        /// Громкость звука.
        /// </summary>
        public float SoundVolume { get; set; }

        /// <summary>
        /// Выбранная локализация.
        /// </summary>
        public SystemLanguage SystemLanguage { get; set; }
    }
}