namespace UnityLib.Architecture.Di.CancellationFactory
{
    using System;
    using System.Threading;

    /// <summary>
    /// Изменяемый источник токена.
    /// </summary>
    /// <remarks>
    /// Оболочка источника, в которой можно зарегистрировать действия по event, и вызвать их после отмены.
    /// </remarks>
    internal sealed class CancellationChangeableSource
    {
        /// <summary>
        /// Источник токена.
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <inheritdoc cref="CancellationChangeableSource" />
        public CancellationChangeableSource()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Токен отменен.
        /// </summary>
        public bool IsCanceled { get; private set; }

        /// <summary>
        /// Токен.
        /// </summary>
        public CancellationToken Token => _cancellationTokenSource.Token;

        /// <summary>
        /// Отменить токен. Источник будет уничтожен.
        /// </summary>
        public void Cancel()
        {
            if (IsCanceled)
                return;

            _cancellationTokenSource.Cancel();
            IsCanceled = true;

            Canceled?.Invoke(Token);
            _cancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Событие, которое вызовится после отмены.
        /// </summary>
        public event Action<CancellationToken> Canceled;

        /// <inheritdoc cref="CancellationChangeableSource" />
        ~CancellationChangeableSource()
        {
            if (IsCanceled)
                return;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}