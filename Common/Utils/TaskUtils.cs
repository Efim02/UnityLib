namespace UnityLib.Common.Utils
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using UnityLib.Common.GO.Logger;

    /// <summary>
    /// Расширение для задач.
    /// </summary>
    public static class TaskUtils
    {
        /// <summary>
        /// Задержка, только можно указать в секундах.
        /// </summary>
        /// <param name="seconds"> Время в секундах. </param>
        public static async Task TaskDelay(float seconds)
        {
            var ms = (int)seconds * 1000;
            await Task.Delay(ms);
        }

        /// <summary>
        /// Задержка, только можно указать в секундах; может быть отменена.
        /// </summary>
        /// <param name="seconds"> Время в секундах. </param>
        /// <param name="cancellationToken"> Токен отмены. </param>
        public static async Task TaskDelay(float seconds, CancellationToken cancellationToken)
        {
            var ms = (int)seconds * 1000;
            await Task.Delay(ms, cancellationToken);
        }

        /// <summary>
        /// Убедиться что во время выполнения задачи не было исключений.
        /// </summary>
        /// <param name="task"> Задача. </param>
        public static async void TryProtect(Task task)
        {
            try
            {
                await task;
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception);
            }
        }

        /// <summary>
        /// Выполнить задачу с отлавливанием исключений. Для синхронных.
        /// </summary>
        /// <param name="action"> Действие. </param>
        /// <returns> Задача, которая ожидается. </returns>
        public static async void TryRun(Action action)
        {
            try
            {
                await Task.Run(action);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception);
            }
        }

        /// <summary>
        /// Выполнить задачу с отлавливанием исключений. Для синхронных.
        /// </summary>
        /// <param name="action"> Действие. </param>
        /// <returns> Задача, которая ожидается. </returns>
        public static async void TryRun<T>(Func<T> action)
        {
            try
            {
                await Task.Run(action.Invoke);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception);
            }
        }

        /// <summary>
        /// Выполнить задачу, запустив ее сразу. Для Асинхронных методов.
        /// </summary>
        /// <param name="func"> Будуща задача. </param>
        /// <returns> Задача, которая ожидается. </returns>
        /// <remarks> Использовать в КРАЙНИХ случаях, когда выполняется логика до асинхрона. </remarks>
        public static void TryRun(Func<Task> func)
        {
            Task.Run(async () =>
            {
                try
                {
                    await func();
                }
                catch (TaskCanceledException)
                {
                }
                catch (Exception exception)
                {
                    GameLogger.Error(exception);
                }
            });
        }
    }
}