namespace UnityLib.Editor.CodeGenerator.Generators
{
    /// <summary>
    /// Данные по контроллерам Ui.
    /// </summary>
    public class ControllerUiData
    {
        private const string CONTROLLER_UIS_PATH_SERVICE_UI = "Scripts/Controllers/ControllerUi";

        private const string NAMESPACE_CONTROLLER = NAMESPACE_CONTROLLERS + ".Controller";
        private const string NAMESPACE_CONTROLLER_ABSTRACTIONS = NAMESPACE_CONTROLLERS + ".Abstractions";
        private const string NAMESPACE_CONTROLLER_UI = NAMESPACE_CONTROLLERS + ".ControllerUi";
        private const string NAMESPACE_CONTROLLERS = "Assets.Scripts.Ui.Controllers";
        private const string NAMESPACE_MVC = "UnityLib.MVC";

        /// <summary>
        /// Данные по контроллерам Ui.
        /// </summary>
        public ControllerUiData()
        {
            ControllerUisPath = CONTROLLER_UIS_PATH_SERVICE_UI;
            NamespaceController = NAMESPACE_CONTROLLER;
            NamespaceControllerAbstractions = NAMESPACE_CONTROLLER_ABSTRACTIONS;
            NamespaceControllers = NAMESPACE_CONTROLLER_UI;
            NamespaceControllerUI = NAMESPACE_CONTROLLERS;
            NamespaceMvc = NAMESPACE_MVC;
        }

        /// <summary>
        /// Путь к папке с контроллерами.
        /// </summary>
        public string ControllerUisPath { get; set; }

        public string NamespaceController { get; set; }
        public string NamespaceControllerAbstractions { get; set; }
        public string NamespaceControllers { get; set; }
        public string NamespaceControllerUI { get; set; }
        public string NamespaceMvc { get; set; }
    }
}