﻿namespace UnityLib.Common.Exceptions
{
    using System;

    using UnityLib.Common.GO.Logger;

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