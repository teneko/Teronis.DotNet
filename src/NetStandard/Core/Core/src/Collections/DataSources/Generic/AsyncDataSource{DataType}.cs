using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Teronis.Collections.DataSources.Generic
{
    public abstract class AsyncDataSource<DataType> : DataSourceBase<DataType>, IAsyncDataSource<DataType>
    {
        private CancellationTokenSource cancellationTokenSource;

        public AsyncDataSource(ILogger? logger = null)
        : base(logger)
            => cancellationTokenSource = new CancellationTokenSource();

        protected abstract IAsyncEnumerable<DataType> EnumerateAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// The source <paramref name="enumerateScopeCancellationTokenSource"/> will be 
        /// registered to <see cref="cancellationTokenSource"/> for being canceled and disposed.
        /// </summary>
        /// <param name="enumerateScopeCancellationTokenSource"></param>
        protected void RegisterCancellationTokenSource(CancellationTokenSource enumerateScopeCancellationTokenSource)
        {
            // When the token source of this class gets canceled/disposed,
            // then also cancel/dispose the token source of this class.
            cancellationTokenSource.Token.Register(() => {
                enumerateScopeCancellationTokenSource?.Cancel();
                enumerateScopeCancellationTokenSource?.Dispose();
            });
        }

        async IAsyncEnumerable<DataType> IAsyncDataSource<DataType>.EnumerateAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            EnsureEnumerate();

            // We link the token of this enumeration to an local token source
            var enumerationCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            RegisterCancellationTokenSource(enumerationCancellationTokenSource);

            EnumerationState = DataSourceEnumerationState.Started;
            var enumerator = EnumerateAsync(enumerationCancellationTokenSource.Token).GetAsyncEnumerator();

            while (true) {
                DataType data;

                try {
                    if (!await enumerator.MoveNextAsync())
                        break;

                    data = enumerator.Current;
                } catch {
                    EnumerationState = DataSourceEnumerationState.Faulted;
                    break;
                }

                yield return data;

                if (cancellationToken.IsCancellationRequested) {
                    EnumerationState = DataSourceEnumerationState.Stopped;
                    break;
                }
            }

            if (EnumerationState == DataSourceEnumerationState.Started)
                EnumerationState = DataSourceEnumerationState.Completed;

            LogEnumerationReachedEnd();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (IsDisposed)
                return;

            // Dispose managed resources
            if (disposing) {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
            }
        }
    }
}
