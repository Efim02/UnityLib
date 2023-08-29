namespace UnityLib.Architecture.Utils
{
    using System;

    using UnityEditor.Search;

    using UnityLib.Architecture.Log;

    public static class DispatcherUtils
    {
        /// <summary>
        /// Вызвать действие с проверкой на исключения, в Dispatcher UI потока.
        /// </summary>
        /// <param name="action"> Действие. </param>
        public static void SafeInvoke(Action action)
        {
            Dispatcher.Enqueue(() =>
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception exception)
                {
                    GameLogger.Error(exception);
                }
            });
        }
    }
}