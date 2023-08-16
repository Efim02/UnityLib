namespace UnityLib.Architecture.Di
{
    using System;

    using UnityLib.Architecture.Log;

    /// <summary>
    /// Исключение об ошибке поиска.
    /// </summary>
    public sealed class ErrorFoundException : Exception
    {
        /// <summary>
        /// Исключение об ошибке поиска.
        /// </summary>
        /// <param name="message"> Сообщение. </param>
        public ErrorFoundException(string message) : base(message)
        {
            GameLogger.Error(message);
        }
    }
}