namespace UnityLib.Core.UI
{
    using System.Text.RegularExpressions;

    using TMPro;

    using UnityEngine;
    using UnityEngine.Events;
    
    using UnityLib.Architecture.Utils;

    /// <summary>
    /// Поле для ввода.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class RegexInputFieldComponent : MonoBehaviour
    {
        [Header("Указать. Regex выражение для поля ввода. Если пустое, значит всё можно вводить.")]
        [SerializeField]
        private string _regexExpression;

        [SerializeField]
        private UnityEvent<string, string> _onChanged;
        
        [SerializeField]
        private UnityEvent<string> _onEnded;

        private TMP_InputField _inputField;
        private string _oldText;

        /// <summary>
        /// Regex выражение для поля ввода.
        /// </summary>
        public string RegexExpression => _regexExpression;

        /// <summary>
        /// Введенный текст.
        /// </summary>
        public string Text
        {
            get => _inputField.text;
            set => _inputField.SetTextWithoutNotify(value);
        }

        private void Awake()
        {
            _inputField = MonoUtils.GetComponent<TMP_InputField>(gameObject);
            _inputField.onValueChanged.AddListener(OnTextChanged);
            _inputField.onEndEdit.AddListener(OnEndEdit);
        }

        /// <summary>
        /// Обрабатывает событие завершения редактирования поля пользователем.
        /// </summary>
        /// <param name="text">Текст.</param>
        private void OnEndEdit(string text)
        {
            _onEnded?.Invoke(text);
        }

        /// <summary>
        /// Изменяет текст, если выражение не указано или Regex выражение выполняется.
        /// </summary>
        /// <param name="newText"> Новый текст. </param>
        private void OnTextChanged(string newText)
        {
            if (string.IsNullOrEmpty(_regexExpression) || Regex.IsMatch(newText, RegexExpression))
            {
                _oldText = _inputField.text;
                _inputField.SetTextWithoutNotify(newText);
                _onChanged?.Invoke(_oldText, newText);
                return;
            }

            _inputField.SetTextWithoutNotify(_oldText);
        }
    }
}