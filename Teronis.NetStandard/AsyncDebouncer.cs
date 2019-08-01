using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis
{
    public class AsyncableDebouncer
    {
        IProcessingDebounce cachedProcessingDebounce;

        public Task<T> Debounce<T>(int interval, Func<Task<T>> debouncedAction)
        {
            if (cachedProcessingDebounce != null)
                cachedProcessingDebounce.TryCancel();

            var processingDebounce = new ProcessingDebounce<T>(interval, debouncedAction);
            cachedProcessingDebounce = processingDebounce;
            return processingDebounce.Start();
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

        private interface IProcessingDebounce
        {
            void TryCancel();
        }

        private enum ProcessingDebounceState
        {
            NotYetStarted,
            Running,
            Stopped,
            Canceled
        }

        private class ProcessingDebounce<T> : IProcessingDebounce
        {
            private Func<Task<T>> debouncedAction;
            private long interval;
            private TaskCompletionSource<T> resultTaskCompletionSource;
            private Stopwatch stopwatch;
            private CancellationTokenSource delayCancellationTokenSource;
            private CancellationToken delayCancellationToken;

            public ProcessingDebounceState State { get; private set; }

            /// <param name="interval">The interval in milliseconds.</param>
            /// <param name="debouncedAction">The function sets gets executed after interval</param>
            public ProcessingDebounce(long interval, Func<Task<T>> debouncedAction)
            {
                stopwatch = new Stopwatch();
                resultTaskCompletionSource = new TaskCompletionSource<T>();
                delayCancellationTokenSource = new CancellationTokenSource();
                delayCancellationToken = delayCancellationTokenSource.Token;
                State = ProcessingDebounceState.NotYetStarted;
                this.debouncedAction = debouncedAction;
                this.interval = interval;
            }

            private void stop()
            {
                if (State == ProcessingDebounceState.Stopped || State == ProcessingDebounceState.Canceled)
                    return;

                stopwatch.Stop();
                State = ProcessingDebounceState.Stopped;
            }

            public async Task<T> Start()
            {
                if (State != ProcessingDebounceState.NotYetStarted)
                    throw new InvalidOperationException("Debounce process has been already started");

                async Task<T> wrappedDebouncedAction()
                {
                    stopwatch.Start();
                    var delayTask = Task.Delay(TimeSpan.FromMilliseconds(interval), delayCancellationToken);
                    State = ProcessingDebounceState.Running;
                    await delayTask;
                    stop();
                    var debouncedActionResult = await debouncedAction();
                    return debouncedActionResult;
                }

                var cancellableDebouncedTask = await Task.WhenAny(new Task<T>[] { resultTaskCompletionSource.Task, wrappedDebouncedAction() });
                var result = await cancellableDebouncedTask;
                return result;
            }

            public void TryCancel()
            {
                if (State == ProcessingDebounceState.Stopped || State == ProcessingDebounceState.Canceled)
                    return;

                stop();

                if (stopwatch.ElapsedMilliseconds < interval) {
                    delayCancellationTokenSource.Cancel();
                    resultTaskCompletionSource.SetException(new OperationCanceledException());
                }
            }
        }
    }
}
