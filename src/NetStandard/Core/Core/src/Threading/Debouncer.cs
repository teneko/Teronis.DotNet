// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Threading
{
    public class Debouncer
    {
        IProcessingDebounce? cachedProcessingDebounce;

        public async Task<T> Debounce<T>(int interval, Func<Task<T>> debouncedAction)
        {
            if (cachedProcessingDebounce != null) {
                await cachedProcessingDebounce.TryCancel();
            }

            var processingDebounce = new ProcessingDebounce<T>(interval, debouncedAction);
            cachedProcessingDebounce = processingDebounce;
            return await processingDebounce.Start();
        }

        public Task Debounce(int interval, Func<Task> debouncedAction)
        {
            async Task<Singleton> wrappedDebouncedAction()
            {
                await debouncedAction();
                return Singleton.Default;
            }

            return Debounce(interval, wrappedDebouncedAction);
        }

        private interface IProcessingDebounce : IDisposable
        {
            Task TryCancel();
        }

        [Flags]
        private enum ProcessingDebounceState
        {
            NotYetStarted = 1,
            Running = 2,
            Stopped = 4,
            Canceled = 8,
        }

        private class ProcessingDebounce<T> : IProcessingDebounce
        {
            public bool IsDisposed { get; private set; }

            private readonly long interval;
            private readonly Func<Task<T>> debouncedAction;
            private readonly Stopwatch stopwatch;
            private readonly CancellationTokenSource delayCancellationTokenSource;
            private readonly CancellationToken processCancellationToken;
            private readonly SemaphoreSlim debouncedActionInvokeLocker;

            public ProcessingDebounceState State { get; private set; }

            /// <param name="interval">The interval in milliseconds.</param>
            /// <param name="debouncedAction">The function gets executed after interval</param>
            public ProcessingDebounce(long interval, Func<Task<T>> debouncedAction)
            {
                stopwatch = new Stopwatch();
                delayCancellationTokenSource = new CancellationTokenSource();
                processCancellationToken = delayCancellationTokenSource.Token;
                debouncedActionInvokeLocker = new SemaphoreSlim(1);
                State = ProcessingDebounceState.NotYetStarted;
                this.debouncedAction = debouncedAction;
                this.interval = interval;
            }

            private void stop()
            {
                if (State == ProcessingDebounceState.Stopped || State == ProcessingDebounceState.Canceled) {
                    return;
                }

                stopwatch.Stop();
                State = ProcessingDebounceState.Stopped;
            }

            public async Task<T> Start()
            {
                try {
                    await debouncedActionInvokeLocker.WaitAsync(processCancellationToken);

                    if (State != ProcessingDebounceState.NotYetStarted) {
                        throw new InvalidOperationException("Debounce process has been already started");
                    }

                    stopwatch.Start();
                    State = ProcessingDebounceState.Running;
                } finally {
                    debouncedActionInvokeLocker.Release();
                }

                await Task.Delay(TimeSpan.FromMilliseconds(interval), processCancellationToken);

                try {
                    await debouncedActionInvokeLocker.WaitAsync(processCancellationToken);
                    stop();
                    return await debouncedAction();
                } finally {
                    debouncedActionInvokeLocker.Release();
                }
            }

            public async Task TryCancel()
            {
                await debouncedActionInvokeLocker.WaitAsync();

                try {
                    if (State == ProcessingDebounceState.Stopped || State == ProcessingDebounceState.Canceled) {
                        return;
                    }

                    if (stopwatch.ElapsedMilliseconds < interval) {
                        stop();
                        delayCancellationTokenSource.Cancel();
                        State = ProcessingDebounceState.Canceled;
                    }
                } finally {
                    debouncedActionInvokeLocker.Release();
                }
            }

            #region IDisposable Support

            protected virtual void Dispose(bool disposing)
            {
                if (!IsDisposed) {
                    if (disposing) {
                        delayCancellationTokenSource.Dispose();
                    }

                    IsDisposed = true;
                }
            }

            // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
            public void Dispose()
                => Dispose(true);

            #endregion
        }
    }
}
