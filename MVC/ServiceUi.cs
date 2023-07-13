namespace UnityLib.MVC
{
    using System;

    using UnityEngine;

    using UnityLib.Common;
    using UnityLib.Common.GO.Logger;

    /// <summary>
    /// Абстракция сервиса.
    /// </summary>
    public class ServiceUi : MonoBehaviour
    {
        /// <summary>
        /// Выполнить безопасно Action.
        /// </summary>
        /// <param name="action"> Действие. </param>
        /// <param name="errorText"> Текст сообщение. </param>
        public void Execute(Action action, string errorText)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                GameLogger.Error(exception, errorText);
            }
        }
    }
}