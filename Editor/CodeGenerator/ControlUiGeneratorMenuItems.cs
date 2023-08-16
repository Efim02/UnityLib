namespace UnityLib.Editor.CodeGenerator
{
    using UnityEditor;
    using UnityEditor.Compilation;

    using UnityLib.Editor.CodeGenerator.Generators;

    public static class ControlUiGeneratorMenuItems
    {
        /// <summary>
        /// Проверить изменения.
        /// TODO: Когда-нибудь реализовать. После реализации убрать InitializeOnLoadMethod из GenerateControlUis.
        /// </summary>
        //[InitializeOnLoadMethod]
        public static void CheckChanges()
        {
        }

        // Полностью регенировать все контроллеры.
        [MenuItem("Игра/Генераторы кода/Удалить контроллеры Ui")]
        public static void DeleteControlUis()
        {
            var controlUiGenerator = new ControlUiGenerator(new ControlUiData());
            controlUiGenerator.DeleteServiceUis();
            AssetDatabase.Refresh();

            CompilationPipeline.RequestScriptCompilation();
        }

        // Запустисть генератор контроллеров.
        [MenuItem("Игра/Генераторы кода/Генерировать контроллеры Ui")]
        [InitializeOnLoadMethod]
        public static void GenerateControlUis()
        {
            var controlUiGenerator = new ControlUiGenerator(new ControlUiData());
            controlUiGenerator.HasChanges();
            controlUiGenerator.Generate();
        }
    }
}