namespace UnityLib.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEngine.SceneManagement;

    /// <summary>
    /// Утилита для работы со сценами.
    /// </summary>
    public static class SceneUtils
    {
        /// <summary>
        /// Получает все имена сцен в проекте.
        /// </summary>
        /// <returns> Имена сцен. </returns>
        public static IEnumerable<string> GetAllSceneNames()
        {
            return GetAllScenePaths().Select(Path.GetFileNameWithoutExtension);
        }

        /// <summary>
        /// Получает все имена сцен в проекте.
        /// </summary>
        /// <returns> Имена сцен. </returns>
        public static IEnumerable<string> GetAllScenePaths()
        {
            for (var index = 0; index < SceneManager.sceneCountInBuildSettings; index++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(index);
                yield return scenePath;
            }
        }
    }
}