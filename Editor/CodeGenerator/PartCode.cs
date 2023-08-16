namespace UnityLib.Editor.CodeGenerator
{
    /// <summary>
    /// Часть кода; контроль отступов.
    /// </summary>
    public enum PartCode
    {
        Namespace,

        Class = 1,

        ClassVariable = 2,

        MethodContent = 3,
    }
}