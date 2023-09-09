namespace UnityLib.Core.Models.Localization
{
    using System;

    using TMPro;

    using UnityEngine;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Компонент для установки корректной надписи, в текстовом компоненте.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizationComponent : MonoBehaviour, ILabel
    {
        /// <summary>
        /// Ключ надписи.
        /// </summary>
        private string _labelKey;

        /// <summary>
        /// Ключ надписи.
        /// </summary>
        public string LabelKey => _labelKey;

        private void Awake()
        {
            _labelKey = GetComponent<TMP_Text>()?.text;
        }

        private void Start()
        {
            if (string.IsNullOrWhiteSpace(LabelKey))
            {
                GameLogger.Error($"Не задан ключ надписи для {gameObject.scene.name}:{name}");
                return;
            }
            
            Injector.Get<Localizer>().AddLabel(this);
            UpdateLabel();
        }

        private void OnDestroy()
        {
            Injector.Get<Localizer>().RemoveLabel(this);
        }

        /// <inheritdoc />
        public void UpdateLabel()
        {
            var textComponent = MonoUtils.GetComponent<TMP_Text>(this);
            textComponent.text = Injector.Get<Localizer>().GetLabel(_labelKey);
        }
    }
}