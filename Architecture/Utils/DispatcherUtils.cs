namespace UnityLib.Architecture.Utils
{
    using System;
    using System.Threading.Tasks;

    using UnityEditor.Search;

    using UnityLib.Architecture.Log;

    public static class DispatcherUtils
    {
        /// <summary>
        /// Вызвать действие с проверкой на исключения, в Dispatcher UI потока.
        /// </summary>
        /// <param name="action"> Действие. </param>
        /// <remarks> Работает без ожидания. </remarks>
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

        /// <summary>
        /// Вызвать действие с проверкой на исключения, в Dispatcher UI потока.
        /// </summary>
        /// <param name="func"> Задача. </param>
        /// <typeparam name="T"> Тип. </typeparam>
        /// <remarks> Работает с задачами и с асинхронным ожиданием. Тяжеловесная работа с тасками (костыль). </remarks>
        public static async Task<T> SafeInvokeAsync<T>(Func<Task<T>> func)
        {
            Exception exception = null;
            Task<T> task = null;

            // ReSharper disable once AsyncVoidLambda
            // Здесь подходящая ситуация.
            Dispatcher.Enqueue(async () =>
            {
                try
                {
                    task = func?.Invoke();
                    if (task == null)
                        throw new ArgumentNullException(nameof(func));

                    await task;
                }
                catch (Exception dispatcherException)
                {
                    exception = dispatcherException;
                }
            });

            while (task == null)
            {
                await Task.Delay(30);
            }

            await Task.WhenAll(task);
            return exception == null ? task.Result : throw exception;
        }
    }
}