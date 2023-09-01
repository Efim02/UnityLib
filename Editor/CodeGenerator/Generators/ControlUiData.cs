namespace UnityLib.Editor.CodeGenerator.Generators
{
    using UnityEngine;

    /// <summary>
    /// Данные по контроллерам Ui.
    /// </summary>
    public class ControlUiData : ScriptableObject
    {
        [SerializeField]
        private string _controlUisPath = "Scripts/Game/Controls/ControlUis";

        [SerializeField]
        private string _namespaceAbstractions = "RootNamespace.Controls.Abstractions";

        [SerializeField]
        private string _namespaceControlUis = "RootNamespace.Controls.ControlUis";

        [SerializeField]
        private string _namespaceImplementations = "RootNamespace.Controls.Implementations";

        [SerializeField]
        private string _namespaceMvc = "UnityLib.Architecture.MVC";

        [SerializeField]
        private string _rootNameSpace = "RootNamespace";

        public string ControlUisPath => _controlUisPath;
        public string NamespaceAbstractions => _namespaceAbstractions;
        public string NamespaceControlUis => _namespaceControlUis;
        public string NamespaceImplementations => _namespaceImplementations;
        public string NamespaceMvc => _namespaceMvc;
        public string RootNameSpace => _rootNameSpace;
    }
}