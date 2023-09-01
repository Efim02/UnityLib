﻿namespace UnityLib.Core.Models.Level
{
    using System;

    /// <summary>
    /// Изменитель уровней.
    /// </summary>
    public interface ILevelChanger
    {
        /// <summary>
        /// Уровень загружен.
        /// </summary>
        public event Action LevelLoaded;

        /// <summary>
        /// Уровень загружается.
        /// </summary>
        public event Action LevelLoading;

        /// <summary>
        /// Загрузить уровень.
        /// </summary>
        /// <param name="gameLevel"> Уровень. </param>
        /// <typeparam name="TLevelEnum"> Тип перечисления уровней. </typeparam>
        public void LoadLevel<TLevelEnum>(TLevelEnum gameLevel) where TLevelEnum : Enum;

        /// <summary>
        /// Загрузить уровень.
        /// </summary>
        /// <param name="gameLevelIndex"> Индекс уровня. </param>
        public void LoadLevel(int gameLevelIndex);
    }
}