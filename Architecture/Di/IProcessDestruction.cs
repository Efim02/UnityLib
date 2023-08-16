namespace UnityLib.Architecture.Di
{
    /// <summary>
    /// Интерфейс обработки уничтожения объекта.
    /// </summary>
    public interface IProcessDestruction
    {
        /// <summary>
        /// Обработать уничтожение.
        /// </summary>
        void ProcessDestruction();
    }
}