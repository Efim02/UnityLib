namespace UnityLib.Architecture.MVC
{
    using System;

    using UnityEngine;

    using UnityLib.Architecture.Log;

    /// <summary>
    /// Абстракция контрола (элемента управления) отвечающего за события из вне.
    /// </summary>
    public class ControlUi : MonoBehaviour
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