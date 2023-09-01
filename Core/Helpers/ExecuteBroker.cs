namespace UnityLib.Core.Helpers
{
    using System;
    using System.Threading.Tasks;

    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Посредник для выполнения дейтвия "единожды", вызванного пользователем.
    /// </summary>
    public class ExecuteBroker
    {
        /// <summary>
        /// Исполняемая задача.
        /// </summary>
        private Task _task;

        /// <summary>
        /// Выполняет асинхронно делегат; в том же потоке в котором был запущен.
        /// </summary>
        /// <param name="func"> Делегат. </param>
        public void Execute(Func<Task> func)
        {
            if (_task != null && _task?.IsCompleted != true)
                return;

            _task = func();
            _ = TaskUtils.TryProtect(_task);
        }
    }
}