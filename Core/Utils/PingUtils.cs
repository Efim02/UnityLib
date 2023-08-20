namespace UnityLib.Core.Utils
{
    using System.Net.NetworkInformation;

    /// <summary>
    /// Утилита для работы с классом <see cref="System.Net.NetworkInformation.Ping" />.
    /// </summary>
    public static class PingUtils
    {
        private const int TIMEOUT = 2000;

        /// <summary>
        /// Проверяет доставку пакетов.
        /// </summary>
        /// <param name="host"> URL адрес хоста, без заголовка: пример <c> firebase.google.com </c>. </param>
        /// <param name="timeOut"> Таймаут. </param>
        /// <returns> TRUE - если пакеты доставлены, иначе - FALSE. </returns>
        public static bool HasPing(string host, int timeOut = TIMEOUT)
        {
            var myPing = new Ping();
            var buffer = new byte[32];
            var pingOptions = new PingOptions();
            var reply = myPing.Send(host, timeOut, buffer, pingOptions);

            return reply?.Status == IPStatus.Success;
        }
    }
}