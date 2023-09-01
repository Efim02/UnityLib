namespace UnityLib.Core.Models.Settings
{
    /// <summary>
    /// Параметры комнаты.
    /// </summary>
    public class RoomParameters
    {
        public RoomParameters()
        {
            MaxPlayers = 2;
        }

        /// <summary>
        /// Максимальное число игроков.
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Комната только для друзей.
        /// </summary>
        public bool OnlyFriends { get; set; }
    }
}