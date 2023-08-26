namespace UnityLib.Editor.CodeGenerator.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using UnityEditor;
    using UnityEditor.Compilation;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.MVC;

    /// <summary>
    /// Генератор контроллеров для удобного использования.
    /// </summary>
    public class ControlUiGenerator
    {
        /// <summary>
        /// Интерфейсы контроллеров.
        /// </summary>
        private readonly List<Type> _abstractions;

        /// <summary>
        /// Данные по контроллерам Ui.
        /// </summary>
        private readonly ControlUiData _controlUiData;

        /// <summary>
        /// Реализации UI контроллеров.
        /// </summary>
        private readonly List<Type> _controlUis;

        /// <summary>
        /// Генератор контроллеров для удобного использования.
        /// </summary>
        /// <param name="controlUiData"> Данные по контроллерам Ui. </param>
        public ControlUiGenerator(ControlUiData controlUiData)
        {
            _controlUiData = controlUiData;

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.IsDynamic)
                .SelectMany(a => a.GetTypes()).ToList();
            _abstractions = types
                .Where(t => t.FullName!
                    .Contains(_controlUiData.NamespaceAbstractions))
                .ToList();
            _controlUis = types
                .Where(t => t.FullName!
                    .Contains(_controlUiData.NamespaceControlUis))
                .ToList();
        }

        /// <summary>
        /// Проверить и генерировать отсутствующие классы.
        /// </summary>
        public void CheckAndGenerate()
        {
            var abstractions = _abstractions.Where(a => _controlUis.Any(c => c.IsSubclassOf(a))).ToList();
            if (!abstractions.Any())
            {
                GameLogger.Info("Новые контролы отсутствуют");
                return;
            }

            GameLogger.Info($"Генерировать недостающие контроллеры Ui: {abstractions.Count}.");

            Generate(abstractions);
        }

        /// <summary>
        /// Удалить контроллеры Ui.
        /// </summary>
        public void DeleteServiceUis()
        {
            var pathServicesUi = Path.Combine(Application.dataPath, _controlUiData.ControlUisPath);
            foreach (var pathServiceUi in Directory.GetFiles(pathServicesUi))
            {
                File.Delete(pathServiceUi);
            }
        }

        /// <summary>
        /// Генерация классов.
        /// </summary>
        public void Regenerate()
        {
            GameLogger.Info($"Регенерация контроллеров Ui: {_abstractions.Count}.");
            Generate(_abstractions);
        }

        /// <summary>
        /// Генерирует по абстракциям файлы контролов Ui.
        /// </summary>
        /// <param name="abstractions"> Абстракции. </param>
        private void Generate(List<Type> abstractions)
        {
            foreach (var abstraction in abstractions)
            {
                var controlName = Path.GetFileNameWithoutExtension(abstraction.Name).Substring(1);
                var controlUiName = $"{controlName}Ui";

                var relativeControlUiPath = $"{_controlUiData.ControlUisPath}/{controlUiName}.cs";
                var controlUiPath = $"{Application.dataPath}/{relativeControlUiPath}";

                Directory.CreateDirectory(Path.GetDirectoryName(controlUiPath)!);
                using (var streamWriter = new StreamWriter(controlUiPath, false, Encoding.UTF8))
                {
                    var generatorHelper = new GeneratorHelper(streamWriter);
                    GenerateFileCs(controlName, controlUiName, abstraction, generatorHelper);
                }

                var pathAssetsRelative = $"Assets/{relativeControlUiPath}";
                AssetDatabase.ImportAsset(pathAssetsRelative);
            }

            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
        }

        /// <summary>
        /// Генерировать один CS файл.
        /// </summary>
        /// <param name="nameService"> Название контроллера. </param>
        /// <param name="nameServiceUi"> Название контроллера UI. </param>
        /// <param name="abstraction"> Абстракция. </param>
        /// <param name="generatorHelper"> Помощник в генерации. </param>
        private void GenerateFileCs(string nameService, string nameServiceUi, Type abstraction,
            GeneratorHelper generatorHelper)
        {
            generatorHelper.NamespaceOpen(_controlUiData.NamespaceControlUis);
            generatorHelper.Using(_controlUiData.NamespaceMvc);
            generatorHelper.Using(_controlUiData.NamespaceAbstractions);
            generatorHelper.Using(_controlUiData.NamespaceImplementations);

            generatorHelper.ClassOpen(nameServiceUi, $"{nameof(ControlUi)}, {abstraction.Name}");
            generatorHelper.ConstWriteLine("string", $"\"Ошибка контроллера {nameServiceUi}\"");
            generatorHelper.VariableWriteline($"private {nameService} _realization;");
            generatorHelper.VariableWriteline($"private {nameService} Realization => _realization ?? " +
                                              $"(_realization = new {nameService}());");

            var methodInfos = abstraction.GetMethods();
            foreach (var methodInfo in methodInfos)
            {
                generatorHelper.MethodOpen(methodInfo);
                WriteMethodBody(generatorHelper, methodInfo);
                generatorHelper.ClosePart(PartCode.ClassVariable);
            }

            generatorHelper.ClosePart(PartCode.Class);
            generatorHelper.ClosePart(PartCode.Namespace);
        }

        /// <summary>
        /// Написать тело для метода.
        /// </summary>
        /// <param name="generatorHelper"> Помощник в генерации. </param>
        /// <param name="methodInfo"> Метода информация. </param>
        private void WriteMethodBody(GeneratorHelper generatorHelper, MethodInfo methodInfo)
        {
            var parametersNames = generatorHelper.GetParamaters(methodInfo);
            generatorHelper.MethodWriteLine($"Execute(() => Realization.{methodInfo.Name}({parametersNames}), ERROR);");
        }
    }
}