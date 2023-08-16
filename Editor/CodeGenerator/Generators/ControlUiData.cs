namespace UnityLib.Editor.CodeGenerator.Generators
{
    using UnityEngine;

    /// <summary>
    /// Данные по контроллерам Ui.
    /// </summary>
    public class ControlUiData : ScriptableObject
    {
        [SerializeField]
        private string _namespaceImplementations = "Controls.Implementations";

        [SerializeField]
        private string _namespaceAbstractions = "Controls.Abstractions";

        [SerializeField]
        private string _namespaceControlUis = "Controls.ControlUis";

        [SerializeField]
        private string _namespaceMvc = "UnityLib.Architecture.MVC";

        [SerializeField]
        private string _controlUisPath = "Scripts/Game/Controls/ControlUis";

        public string ControlUisPath => _controlUisPath;

        public string NamespaceImplementations => _namespaceImplementations;

        public string NamespaceAbstractions => _namespaceAbstractions;

        public string NamespaceControlUis => _namespaceControlUis;

        public string NamespaceMvc => _namespaceMvc;
    }
}