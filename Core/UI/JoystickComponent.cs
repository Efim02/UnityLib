namespace Assets.Scripts.UI.Game.Control
{
    using System;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// Джойстик. Эмитация работы джойстика.
    /// </summary>
    public class JoystickComponent : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Просмотр. Направление Joystick")]
        [SerializeField()]
        private Vector2 _inputVector2;

        /// <summary>
        /// Задний фон джойстика.
        /// </summary>
        [Header("Указать")]
        public RectTransform BgRectTransform;

        /// <summary>
        /// Движемый круглишок.
        /// </summary>
        public Image TouchMarker;

        /// <summary>
        /// Вернет вектор, который получен прямиком с джойстика.
        /// </summary>
        public Vector2 InputVector2 { get => _inputVector2; private set => _inputVector2 = value; }

        /// <summary>
        /// Вернет вектор, который получен из <see cref="InputVector2" /> только "y" установлен в "z".
        /// </summary>
        public Vector3 InputVector3 => new Vector3(InputVector2.x, 0, InputVector2.y);

        public virtual void OnDrag(PointerEventData pointer)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(BgRectTransform,
                    pointer.position,
                    pointer.pressEventCamera,
                    out var pos))
                return;

            var joystickBgSizeX = BgRectTransform.sizeDelta.x;
            var joystickBgSizeY = BgRectTransform.sizeDelta.y;

            // По умолчанию Pivot (0,0), но мы учитываем что этом может быть не так.
            pos.x += joystickBgSizeX;
            pos.y += joystickBgSizeY;

            pos.x /= joystickBgSizeX;
            pos.y /= joystickBgSizeY;

            InputVector2 = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            InputVector2 = InputVector2.magnitude > 1.0f ? InputVector2.normalized : InputVector2;

            // Смещаем маркер.
            TouchMarker.rectTransform.anchoredPosition = new Vector2(
                InputVector2.x * (joystickBgSizeX / 2),
                InputVector2.y * (joystickBgSizeY / 2));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            InputVector2 = Vector2.zero;
            TouchMarker.rectTransform.anchoredPosition = Vector2.zero;
            OnPointerUpped?.Invoke();
        }

        /// <summary>
        /// Прикосновение было закончено.
        /// </summary>
        public event Action OnPointerUpped;
    }
}