namespace UnityLib.Core.Models.Level
{
    using System;

    /// <summary>
    /// Изменитель уровней.
    /// </summary>
    public interface ISceneLoader
    {
        /// <summary>
        /// Текущий индекс сцены.
        /// </summary>
        int CurrentSceneIndex { get; }

        /// <summary>
        /// Предыдущий индекс сцены.
        /// </summary>
        int PreviousSceneIndex { get; }

        /// <summary>
        /// Получает текущую сцену.
        /// </summary>
        /// <typeparam name="TScene"> Тип перечисления. </typeparam>
        /// <returns> Сцена. </returns>
        TScene GetCurrentScene<TScene>() where TScene : struct;

        /// <summary>
        /// Получает предыдущую сцену.
        /// </summary>
        /// <typeparam name="TScene"> Тип перечисления. </typeparam>
        /// <returns> Сцена. </returns>
        TScene GetPreviousScene<TScene>() where TScene : struct;

        /// <summary>
        /// Сцена загружена.
        /// </summary>
        event Action LevelLoaded;

        /// <summary>
        /// Сцена загружается.
        /// </summary>
        event Action LevelLoading;

        /// <summary>
        /// Загрузить сцену.
        /// </summary>
        /// <param name="scene"> Сцена. </param>
        /// <typeparam name="TLevelEnum"> Тип перечисления сцен. </typeparam>
        void LoadScene<TLevelEnum>(TLevelEnum scene) where TLevelEnum : Enum;

        /// <summary>
        /// Загрузить сцену.
        /// </summary>
        /// <param name="gameLevelIndex"> Индекс сцены. </param>
        void LoadScene(int gameLevelIndex);
    }
}