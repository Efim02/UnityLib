namespace UnityLib.Architecture.Models.Scene
{
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Информация сцены.
    /// </summary>
    public class SceneInfo
    {
        public SceneInfo(Scene scene) : this(scene.buildIndex, scene.name, scene.path)
        {
        }

        private SceneInfo(int index, string name, string path)
        {
            Index = index;
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Индекс.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Путь.
        /// </summary>
        public string Path { get; private set; }
    }
}