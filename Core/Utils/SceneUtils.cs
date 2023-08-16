namespace UnityLib.Core.Utils
{
    using System.Collections.Generic;

    using UnityEngine.SceneManagement;

    /// <summary>
    /// Утилита для работы со сценами.
    /// </summary>
    public static class SceneUtils
    {
        /// <summary>
        /// Получает все сцены в проекте.
        /// </summary>
        /// <returns> Сцены. </returns>
        public static IEnumerable<Scene> GetAllScenes()
        {
            for (var index = 0; index < SceneManager.sceneCountInBuildSettings; index++)
            {
                yield return SceneManager.GetSceneAt(index);
            }
        }
    }
}