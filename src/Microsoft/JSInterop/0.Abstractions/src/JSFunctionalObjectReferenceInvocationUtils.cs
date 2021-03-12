using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Teronis.Microsoft.JSInterop
{
    public static class JSFunctionalObjectInvocationUtils
    {
        public static bool CreateValueTaskSourceIfCancellationRequested<TValue>(CancellationToken? cancellationToken, TimeSpan? timeout, [MaybeNullWhen(false)]out ExceptionValueTaskSource<TValue> valueTaskSource) {
            if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested) {
                valueTaskSource = new ExceptionValueTaskSource<TValue>(new OperationCanceledException(cancellationToken.Value));
                return true;
            } else if (timeout.HasValue && timeout.Value == TimeSpan.Zero) {
                valueTaskSource = new ExceptionValueTaskSource<TValue>(new OperationCanceledException());
                return true;
            }

            valueTaskSource = null;
            return false;
        }
    }
}
