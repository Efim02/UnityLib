namespace UnityLib.Core.Models.Localization
{
    using TMPro;

    using UnityEngine;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Компоненет для установки корректной надписи, в текстовом компоненте.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizationComponent : MonoBehaviour, ILabel
    {
        /// <summary>
        /// Ключ надписи.
        /// </summary>
        [Header("Указать. Ключ надписи.")]
        [SerializeField]
        private string _labelKey;

        /// <summary>
        /// Ключ надписи.
        /// </summary>
        public string LabelKey => _labelKey;

        private void Start()
        {
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