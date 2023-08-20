namespace UnityLib.Core.Controllers
{
    using System;

    using UnityEngine;

    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Utils;
    using UnityLib.Core.Helpers.Fields;

    /// <summary>
    /// Абстрактный класс для работы аудио контроллера.
    /// </summary>
    /// <typeparam name="TEnum"> Тип перечисления. </typeparam>
    public abstract class AbstractAudioPlayer<TEnum> : MonoBehaviour
        where TEnum : Enum
    {
        /// <summary>
        /// Словарь звуков и треков.
        /// </summary>
        [SerializeField]
        private MapList<TEnum, AudioClip> _soundtracks;

        /// <summary>
        /// Источник звука.
        /// </summary>
        protected AudioSource AudioSource;

        protected virtual void Awake()
        {
            AudioSource = MonoUtils.GetComponent<AudioSource>(this);
        }

        /// <summary>
        /// Проиграть на источнике.
        /// </summary>
        protected virtual void PlayOnSource(TEnum typeSound)
        {
            var audioClip = _soundtracks[typeSound];

            AudioSource.Stop();
            AudioSource.PlayOneShot(audioClip);
        }

        /// <summary>
        /// Попробовать получить трек.
        /// </summary>
        /// <param name="someEnum"> Тип из перечисления. </param>
        /// <param name="audioClip"> Трек. </param>
        /// <returns> </returns>
        protected bool TryGetClip(TEnum someEnum, out AudioClip audioClip)
        {
            var result = _soundtracks.TryGetValue(someEnum, out audioClip);
            if (!result)
                GameLogger.Error($"Нет трека {someEnum}, в {GetType().Name}.");

            return result;
        }
    }
}