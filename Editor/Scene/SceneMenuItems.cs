namespace UnityLib.Editor.Scene
{
    using System.Linq;

    using UnityEditor;
    using UnityEditor.SceneManagement;

    using UnityEngine.SceneManagement;

    using UnityLib.Core.Utils;

    /// <summary>
    /// Меню для работы со сценами.
    /// </summary>
    public class SceneMenuItems
    {
        private const string LAST_SCENE_PATH = "last_scene_path";

        [MenuItem("Игра/Вернуться на сцену")]
        public static void ComeBackGame()
        {
            var path = PersistentPrefs.Get<string>(LAST_SCENE_PATH);
            EditorSceneManager.OpenScene(path);
        }

        [MenuItem("Игра/Запустить игру")]
        public static void StartGame()
        {
            var scene = SaveScene();

            PersistentPrefs.Set(LAST_SCENE_PATH, scene.path);
            EditorSceneManager.OpenScene(SceneUtils.GetAllScenePaths().ToList()[0]);
            EditorApplication.EnterPlaymode();
        }

        private static Scene SaveScene()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.isDirty)
                EditorSceneManager.SaveScene(scene);

            return scene;
        }

        #region Загрузить уровень X
        
        private static void LoadLevel(int index)
        {
            SaveScene();
            EditorSceneManager.OpenScene(SceneUtils.GetAllScenePaths().ToList()[index]);
        }

        [MenuItem("Игра/Карта/Загрузить уровень 0")]
        public static void LoadLevel0()
        {
            LoadLevel(0);
        }

        [MenuItem("Игра/Карта/Загрузить уровень 1")]
        public static void LoadLevel1()
        {
            LoadLevel(1);
        }

        [MenuItem("Игра/Карта/Загрузить уровень 2")]
        public static void LoadLevel2()
        {
            LoadLevel(2);
        }

        [MenuItem("Игра/Карта/Загрузить уровень 3")]
        public static void LoadLevel3()
        {
            LoadLevel(3);
        }

        [MenuItem("Игра/Карта/Загрузить уровень 4")]
        public static void LoadLevel4()
        {
            LoadLevel(4);
        }

        #endregion
    }
}