namespace UnityLib.Architecture.Models.OUS
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using UnityLib.Architecture.Di;

    /// <summary>
    /// Контроллер для единой точки запуска и обновления объектов.
    /// </summary>
    public class OusController : MonoBehaviour
    {
        private GameObject[] _allObjects;

        /// <summary>
        /// Запускаемые объекты.
        /// </summary>
        private List<IStart> _starters;

        /// <summary>
        /// Обновляемые объекты.
        /// </summary>
        private List<IUpdate> _updates;

        public void Awake()
        {
            Injector.RebindSingleton(this, true);

            _allObjects = FindObjectsOfType<GameObject>();
            _starters = GetAllComponents<IStart>();
            _updates = GetAllComponents<IUpdate>();
        }

        public virtual void Start()
        {
            _starters.ForEach(s => s.Start());
        }

        public virtual void Update()
        {
            _updates.ForEach(s => s.Update());
        }

        /// <summary>
        /// Получает все компоненты объектов.
        /// </summary>
        /// <typeparam name="T"> Тип наследумый компонентов. </typeparam>
        /// <returns> Список компонентов. </returns>
        private List<T> GetAllComponents<T>()
        {
            return _allObjects.SelectMany(o => o.GetComponents<T>()).ToList();
        }
    }
}