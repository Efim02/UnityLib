namespace UnityLib.Editor.CodeGenerator
{
    using System.IO;

    using UnityEditor;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Editor.CodeGenerator.Generators;

    public static class ControlUiGeneratorMenuItems
    {
        private const string CONTROL_UI_DATA_PATH = @"Assets/Scripts/Editor/Resources/ControlUiData.asset";

        [MenuItem("Игра/Генераторы кода/Создать конфиг")]
        public static void CreateControlUiData()
        {
            if (File.Exists(CONTROL_UI_DATA_PATH))
                File.Delete(CONTROL_UI_DATA_PATH);

            Directory.CreateDirectory(Path.GetDirectoryName(CONTROL_UI_DATA_PATH)!);
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ControlUiData>(), CONTROL_UI_DATA_PATH);
        }

        // Полностью регенировать все контроллеры.
        [MenuItem("Игра/Генераторы кода/Регенерировать контролы Ui")]
        public static void DeleteControlUis()
        {
            var controlUiGenerator = new ControlUiGenerator(GetConfig());
            controlUiGenerator.Regenerate();
        }

        // Запустисть генератор контроллеров.
        [MenuItem("Игра/Генераторы кода/Проверить и генерировать недостающие контролы")]
        public static void GenerateControlUis()
        {
            var controlUiGenerator = new ControlUiGenerator(GetConfig());
            controlUiGenerator.CheckAndGenerate();
        }

        [MenuItem("Игра/Генераторы кода/Выбрать конфиг")]
        public static void SelectControlUiData()
        {
            if (!File.Exists(CONTROL_UI_DATA_PATH))
                GameLogger.Warning("Необходимо сначала создать файл конфига");

            Selection.activeObject = GetConfig();
        }

        /// <summary>
        /// Получает конфиг для генератора кода.
        /// </summary>
        /// <returns> Конфиг. </returns>
        private static ControlUiData GetConfig()
        {
            return (ControlUiData)AssetDatabase.LoadMainAssetAtPath(CONTROL_UI_DATA_PATH);
        }

        #region CodeДляОтлавливанияОшибокСборки

        /* 
         class CustomBuildPipeline : MonoBehaviour, IPreprocessBuildWithReport, IPostprocessBuildWithReport
   {
       public int callbackOrder => 0;

       // CALLED BEFORE THE BUILD
       public void OnPreprocessBuild(BuildReport report)
       {
           // Start listening for errors when build starts
           Application.logMessageReceived += OnBuildError;
       }

       // CALLED DURING BUILD TO CHECK FOR ERRORS
       private void OnBuildError(string condition, string stacktrace, LogType type)
       {
           if (type == LogType.Error)
           {
               // FAILED TO BUILD, STOP LISTENING FOR ERRORS
               Application.logMessageReceived -= OnBuildError;
           }
       }

       // CALLED AFTER THE BUILD
       public void OnPostprocessBuild(BuildReport report)
       {
           // IF BUILD FINISHED AND SUCCEEDED, STOP LOOKING FOR ERRORS
           Application.logMessageReceived -= OnBuildError;
       }
   }
         */

        #endregion
    }
}