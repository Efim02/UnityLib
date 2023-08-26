namespace UnityLib.Architecture.MVC
{
    /// <summary>
    /// Абстракция одного из представлений для модели, которая автоматически подключается к модели.
    /// </summary>
    /// <typeparam name="TModel"> Модель. </typeparam>
    public abstract class AutoViewUi<TModel> : ViewUi<TModel>
        where TModel : SingleModel
    {
        /// <summary>
        /// Зарегистрировано ли представление.
        /// </summary>
        private bool _isRegistered;

        /// <summary>
        /// Нужно ли обновить представление, с текущими значениями модели,
        /// при инициализации "Start" представления.
        /// </summary>
        public virtual bool IsNeedUpdateView => true;

        protected void Start()
        {
            InitializeView();
        }

        /// <summary>
        /// Удаление объекта, и удаление представления, для модели.
        /// </summary>
        /// <remarks> Этого не произойдет если представление ни разу не будет активировано. </remarks>
        public virtual void OnDestroy()
        {
            ViewModelConnector.RemoveView<TModel>(this);
        }

        /// <summary>
        /// Создание объекта, и добавление представления, для модели.
        /// </summary>
        public sealed override void InitializeView()
        {
            if (_isRegistered)
                return;

            // Если представление ни разу не будет активировано, OnDestroy не отработает.
            gameObject.SetActive(true);
            gameObject.SetActive(false);

            _isRegistered = true;
            if (IsNeedUpdateView)
                ViewModelConnector.InitializeView<TModel>(this);

            ViewModelConnector.AddView<TModel>(this);
            InitializeAutoView();
        }

        /// <summary>
        /// Инициализация представления.
        /// </summary>
        /// <remarks> Реализовать, если необходим такой callback. </remarks>
        protected virtual void InitializeAutoView()
        {
        }
    }
}