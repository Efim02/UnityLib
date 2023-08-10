namespace UnityLib.Di.CancellationFactory
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Фабрика токенов для отмены задач.
    /// </summary>
    internal sealed class CancellationTokenFactory
    {
        /// <summary>
        /// Словарь изменяемых токенов.
        /// <para>
        ///     <c> Key: CancellationToken. Value: ChangeableSource. </c>
        /// </para>
        /// </summary>
        private readonly Dictionary<CancellationToken, CancellationChangeableSource> _dictionaryChangeableSources;

        /// <summary>
        /// Основной источник который хранит токен для отмены задач на игровом уровне,
        /// при его выгрузки.
        /// </summary>
        private CancellationTokenSource _cancellationSourceOfScene;

        /// <inheritdoc cref="CancellationTokenFactory" />
        public CancellationTokenFactory()
        {
            _cancellationSourceOfScene = new CancellationTokenSource();
            _dictionaryChangeableSources = new Dictionary<CancellationToken, CancellationChangeableSource>();
        }

        /// <summary>
        /// Отменить токены на уровне.
        /// </summary>
        public void CancelSceneTokens()
        {
            // Отменяем токен, который вызывается по выгрузки уровня.
            _cancellationSourceOfScene.Cancel();
            _cancellationSourceOfScene.Dispose();

            _cancellationSourceOfScene = new CancellationTokenSource();

            // Удаляем изменяемые источники.
            foreach (var source in _dictionaryChangeableSources.Values)
            {
                source.Canceled -= RemoveCancelledToken;
                source.Cancel();
            }

            _dictionaryChangeableSources.Clear();
        }

        /// <summary>
        /// Получить источник токена, для отмены задач.
        /// Этот источник может быть изменен, как при каком-то обстоятельстве на сцене,
        /// так и при отмене его в фактории.
        /// </summary>
        /// <returns> Источник токена отмены. </returns>
        /// <remarks>
        /// При отмене вручную, уничтожается источник <see cref="CancellationTokenSource.Dispose" />.
        /// </remarks>
        public CancellationChangeableSource GetSceneChangeableSource()
        {
            var changeableTokenSource = new CancellationChangeableSource();
            _dictionaryChangeableSources.Add(changeableTokenSource.Token, changeableTokenSource);

            changeableTokenSource.Canceled += RemoveCancelledToken;
            return changeableTokenSource;
        }

        /// <summary>
        /// Получить токен для отмены задачи на сцене.
        /// </summary>
        /// <returns> Токен. </returns>
        public CancellationToken GetSceneToken()
        {
            return _cancellationSourceOfScene.Token;
        }

        /// <summary>
        /// Удалить запись о токене, который отменен.
        /// </summary>
        /// <param name="cancellationToken"> Отмененный токен. </param>
        private void RemoveCancelledToken(CancellationToken cancellationToken)
        {
            _dictionaryChangeableSources.Remove(cancellationToken);
        }

        /// <summary>
        /// Нужно для удаления неизменяемого токена уровня.
        /// </summary>
        ~CancellationTokenFactory()
        {
            _cancellationSourceOfScene.Cancel();
            _cancellationSourceOfScene.Dispose();
        }
    }
}