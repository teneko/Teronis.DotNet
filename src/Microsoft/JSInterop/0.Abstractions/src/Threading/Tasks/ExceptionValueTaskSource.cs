using System;
using System.Threading.Tasks.Sources;

namespace Teronis.Microsoft.JSInterop.Threading.Tasks
{
    public class ExceptionValueTaskSource : IValueTaskSource
    {
        protected Exception Exception { get; }

        public ExceptionValueTaskSource(Exception exception) =>
            Exception = exception;

        public void GetResult(short _) =>
            throw Exception;

        public ValueTaskSourceStatus GetStatus(short token) =>
            ValueTaskSourceStatus.Faulted;

        void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        { }
    }
}
