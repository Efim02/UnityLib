namespace UnityLib.Editor.CodeGenerator.Generators
{
    /// <summary>
    /// Данные по контроллерам Ui.
    /// </summary>
    public class ControlUiData
    {
        private const string CONTROLLER_UIS_PATH_SERVICE_UI = "Scripts/Game/Controls/ControlUis";

        private const string NAMESPACE_IMPLEMENTATIONS = NAMESPACE_CONTROLS + ".Implementations";
        private const string NAMESPACE_CONTROL_ABSTRACTIONS = NAMESPACE_CONTROLS + ".Abstractions";
        private const string NAMESPACE_CONTROL_UI = NAMESPACE_CONTROLS + ".ControlUis";
        private const string NAMESPACE_CONTROLS = "Controls";
        private const string NAMESPACE_MVC = "UnityLib.Architecture.MVC";

        /// <summary>
        /// Данные по контроллерам Ui.
        /// </summary>
        public ControlUiData()
        {
            ControllerUisPath = CONTROLLER_UIS_PATH_SERVICE_UI;
            NamespaceController = NAMESPACE_IMPLEMENTATIONS;
            NamespaceControllerAbstractions = NAMESPACE_CONTROL_ABSTRACTIONS;
            NamespaceControllers = NAMESPACE_CONTROL_UI;
            NamespaceControllerUI = NAMESPACE_CONTROLS;
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