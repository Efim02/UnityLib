namespace UnityLib.Core.Helpers
{
    using System;
    using System.Threading.Tasks;

    using UnityLib.Architecture.Utils;

    /// <summary>
    /// ��������� ��� ���������� ������� "��������", ���������� �������������.
    /// </summary>
    public class ExecuteBroker
    {
        /// <summary>
        /// ����������� ������.
        /// </summary>
        private Task _task;

        /// <summary>
        /// ��������� ���������� �������; � ��� �� ������ � ������� ��� �������.
        /// </summary>
        /// <param name="func"> �������. </param>
        public void Execute(Func<Task> func)
        {
            if (_task != null && _task?.IsCompleted != true)
                return;

            _task = func();
            _ = TaskUtils.TryProtect(_task);
        }
    }
}