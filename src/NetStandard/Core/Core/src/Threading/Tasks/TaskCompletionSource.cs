using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Threading.Tasks
{
    public class TaskCompletionSource
    {
        public Task Task => tcs.Task;

        private TaskCompletionSource<object?> tcs;

        public TaskCompletionSource() => tcs = new TaskCompletionSource<object?>();
        public TaskCompletionSource(object state) => tcs = new TaskCompletionSource<object?>(state);
        public TaskCompletionSource(TaskCreationOptions creationOptions) => tcs = new TaskCompletionSource<object?>(creationOptions);
        public TaskCompletionSource(object state, TaskCreationOptions creationOptions) => tcs = new TaskCompletionSource<object?>(state, creationOptions);

        public void SetResult() => tcs.SetResult(null);
        public void SetCanceled() => tcs.SetCanceled();
        public void SetException(Exception error) => tcs.SetException(error);
        public void SetException(IEnumerable<Exception> errors) => tcs.SetException(errors);
        public bool TrySetResult() => tcs.TrySetResult(null);
        public bool TrySetCanceled() => tcs.TrySetCanceled();
        public bool TrySetCanceled(CancellationToken cancellationToken) => tcs.TrySetCanceled(cancellationToken);
        public bool TrySetException(Exception error) => tcs.TrySetException(error);
        public void TrySetException(IEnumerable<Exception> errors) => tcs.TrySetException(errors);
    }
}
