namespace UnityLib.Core.Utils
{
    using System;

    using UnityLib.Architecture.Log;

    public static class ExecuteUtils
    {
        /// <summary>
        /// Выполняет с проверкой с отлавливанием исключений.
        /// </summary>
        /// <param name="action"> </param>
        public static void SafeExecute(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception);
            }
        }
    }
}