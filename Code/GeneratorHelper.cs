namespace UnityLib.Code
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Утилита для записи.
    /// </summary>
    public class GeneratorHelper
    {
        /// <summary>
        /// Записыватель.
        /// </summary>
        private readonly StreamWriter _streamWriter;

        /// <summary>
        /// Отступы.
        /// </summary>
        private string _tabs;

        /// <summary>
        /// Количестов отступов.
        /// </summary>
        private int _tabsCount;

        /// <inheritdoc cref="GeneratorHelper" />
        public GeneratorHelper(StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
        }

        public void ClassOpen(string className)
        {
            SetTabs(PartCode.Class);
            WriteLine($"public class {className}");
            WriteOpenPart();
        }

        public void ClassOpen(string className, string textAbstractions)
        {
            SetTabs(PartCode.Class);
            WriteLine($"public class {className} : {textAbstractions}");
            WriteOpenPart();
        }

        public void ClosePart(PartCode partCode)
        {
            SetTabs(partCode);
            WriteClosePart();
        }

        public void ConstWriteLine(string type, string value)
        {
            SetTabs(PartCode.ClassVariable);
            WriteLine($"public const {type} ERROR = {value};");
        }

        /// <summary>
        /// Получить названия параметров для вызываемого метода.
        /// </summary>
        /// <param name="methodInfo"> Метод инфо. </param>
        public string GetParamaters(MethodInfo methodInfo)
        {
            var content = string.Empty;
            var parameters = methodInfo.GetParameters();

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                if (index != parameters.Length - 1)
                {
                    content += $"{parameter.Name}, ";
                    continue;
                }

                content += $"{parameter.Name}";
            }

            return content;
        }

        public void MethodOpen(MethodInfo methodInfo)
        {
            SetTabs(PartCode.ClassVariable);
            var parameters = GetParametersWithTypes(methodInfo);
            WriteLine($"public void {methodInfo.Name}({parameters})");
            OpenPart(PartCode.ClassVariable);
        }

        public void MethodWriteLine(string line)
        {
            SetTabs(PartCode.MethodContent);
            WriteLine(line);
        }

        public void MonoClassOpen(string className)
        {
            SetTabs(PartCode.Class);
            WriteLine($"public class {className} : MonoBehaviour");
            WriteOpenPart();
        }

        public void MonoClassOpen(string className, string abstraction)
        {
            SetTabs(PartCode.Class);
            WriteLine($"public class {className} : MonoBehaviour, {abstraction}");
            SetTabs(PartCode.Class);
        }

        public void NamespaceOpen(string namespaceName)
        {
            SetTabs(PartCode.Namespace);
            WriteLine($"namespace {namespaceName}");
            WriteOpenPart();
        }

        public void OpenPart(PartCode partCode)
        {
            SetTabs(partCode);
            WriteOpenPart();
        }

        public void Using(string assembly)
        {
            SetTabs(PartCode.Class);
            WriteLine($"using {assembly};");
        }

        public void VariableWriteline(string variableText)
        {
            SetTabs(PartCode.ClassVariable);
            WriteLine(variableText);
        }


        /// <summary>
        /// Получить названия параметров для создания метода.
        /// </summary>
        /// <param name="methodInfo"> Метод инфо. </param>
        private string GetParametersWithTypes(MethodInfo methodInfo)
        {
            var content = string.Empty;
            var parameters = methodInfo.GetParameters();

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                if (index != parameters.Length - 1)
                {
                    content += $"{parameter.ParameterType} {parameter.Name}, ";
                    continue;
                }

                content += $"{parameter.ParameterType} {parameter.Name}";
            }

            return content;
        }

        /// <summary>
        /// Устновить отступы.
        /// </summary>
        /// <param name="count"> Количество. </param>
        private void SetTabs(PartCode partCode)
        {
            var count = (int)partCode;
            if (_tabsCount == count)
                return;

            _tabsCount = count;
            _tabs = string.Empty;
            for (; count > 0; count--)
                _tabs += "\t";
        }

        private void WriteClosePart()
        {
            WriteLine("}");
        }

        private void WriteLine(string text)
        {
            _streamWriter.WriteLine($"{_tabs}{text}");
        }

        private void WriteOpenPart()
        {
            WriteLine("{");
        }
    }
}