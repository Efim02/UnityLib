namespace UnityLib.Editor.CodeGenerator.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using UnityEditor;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.MVC;

    /// <summary>
    /// Генератор контроллеров для удобного использования.
    /// </summary>
    public class ControlUiGenerator : GeneratorBase
    {
        /// <summary>
        /// Данные по контроллерам Ui.
        /// </summary>
        private readonly ControlUiData _controlUiData;

        /// <summary>
        /// Интерфейсы контроллеров.
        /// </summary>
        private Type[] _serviceInterfaces;

        /// <summary>
        /// Генератор контроллеров для удобного использования.
        /// </summary>
        /// <param name="controlUiData"> Данные по контроллерам Ui. </param>
        public ControlUiGenerator(ControlUiData controlUiData)
        {
            _controlUiData = controlUiData;
        }
        
        /// <inheritdoc />
        public override bool IsCached { get; set; }

        /// <summary>
        /// Удалить контроллеры Ui.
        /// </summary>
        public void DeleteServiceUis()
        {
            var pathServicesUi = Path.Combine(Application.dataPath, _controlUiData.ControllerUisPath);
            foreach (var pathServiceUi in Directory.GetFiles(pathServicesUi))
            {
                File.Delete(pathServiceUi);
            }
        }

        /// <summary>
        /// Генерация классов.
        /// </summary>
        public override void Generate()
        {
            GameLogger.Info($"Генерация контроллеров Ui {nameof(ControlUiGenerator)}: {_serviceInterfaces.Length}.");

            foreach (var serviceInterace in _serviceInterfaces)
            {
                var nameService = Path.GetFileNameWithoutExtension(serviceInterace.Name).Substring(1);
                var nameServiceUi = $"{nameService}Ui";

                var pathRelative = $"{_controlUiData.ControllerUisPath}/{nameServiceUi}.cs";
                var pathServiceUi = $"{Application.dataPath}/{pathRelative}";

                using (var streamWriter = new StreamWriter(pathServiceUi, false, Encoding.UTF8))
                {
                    var generatorHelper = new GeneratorHelper(streamWriter);
                    GenerateFileCs(nameService, nameServiceUi, serviceInterace, generatorHelper);
                }

                var pathAssetsRelative = $"Assets/{pathRelative}";
                AssetDatabase.ImportAsset(pathAssetsRelative);
            }
        }

        /// <inheritdoc />
        public override bool HasChanges()
        {
            IsCached = true;

            var servicesTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes)
                .Where(t => t.FullName!.Contains(_controlUiData.NamespaceControllers)).ToList();

            _serviceInterfaces = servicesTypes
                .Where(t => t.Namespace == _controlUiData.NamespaceControllerAbstractions)
                .ToArray();
            var serviceUis = servicesTypes.Where(t => t.Namespace == _controlUiData.NamespaceControllerUI).ToArray();

            var serviceInterfacesResult = new List<Type>();
            // Проверяем надо ли обновлять файлы.
            foreach (var serviceInterface in _serviceInterfaces)
            {
                var serviceUi = serviceUis.FirstOrDefault(sui => sui.GetInterface(serviceInterface.Name) != null);
                if (serviceUi == null)
                    serviceInterfacesResult.Add(serviceInterface);

                // Не умеет получать новые методы, без рекомпиляции.
                //var serviceUiMethods = serviceUi.GetMethods();
                //var needAdd = serviceInterface.GetMethods().All(im => serviceUiMethods.Any(sm =>
                //{
                //    var equalsNames = im.Name == sm.Name;
                //    var eqaulsParams = im.GetParameters().Length == sm.GetParameters().Length;
                //    return eqaulsParams && equalsNames;
                //}));

                //if (!needAdd)
                //    serviceInterfacesResult.Add(serviceInterface);
            }

            _serviceInterfaces = serviceInterfacesResult.ToArray();
            return _serviceInterfaces.Any();
        }

        /// <summary>
        /// Устновить интерфейсы.
        /// </summary>
        public void SetInterfaces()
        {
            var servicesTypes =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes)
                    .Where(t => t.FullName!.Contains(_controlUiData.NamespaceControllers));

            _serviceInterfaces = servicesTypes
                .Where(t => t.Namespace == _controlUiData.NamespaceControllerAbstractions)
                .ToArray();
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
            generatorHelper.NamespaceOpen(_controlUiData.NamespaceControllerUI);
            generatorHelper.Using(_controlUiData.NamespaceMvc);
            generatorHelper.Using(_controlUiData.NamespaceControllerAbstractions);
            generatorHelper.Using(_controlUiData.NamespaceController);

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