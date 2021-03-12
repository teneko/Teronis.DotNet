using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public ref struct JSFunctionalObjectInvocation
    {
        public bool CanInvoke => promise.HasValue;

        public readonly IJSObjectReference JSObjectReference { get; }
        public readonly string Identifier { get; }
        public readonly CancellationToken? CancellationToken { get; }
        public readonly TimeSpan? TimeSpan { get; }
        public readonly object?[] Arguments { get; }

        private ValueTask? promise;

        internal JSFunctionalObjectInvocation(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeSpan,
            object?[] arguments)
        {
            JSObjectReference = jSObjectReference;
            Identifier = identifier;
            CancellationToken = cancellationToken;
            TimeSpan = timeSpan;
            Arguments = arguments;
            promise = default;
        }

        public ValueTask InvokeAsync()
        {
            if (!promise.HasValue) {
                if (CancellationToken.HasValue) {
                    promise = JSObjectReferenceExtensions.InvokeVoidAsync(JSObjectReference, Identifier, cancellationToken: CancellationToken.Value, Arguments);
                    ;
                } else if (TimeSpan.HasValue) {
                    promise = JSObjectReferenceExtensions.InvokeVoidAsync(JSObjectReference, Identifier, timeout: TimeSpan.Value, Arguments);
                } else {
                    promise = JSObjectReferenceExtensions.InvokeVoidAsync(JSObjectReference, Identifier, Arguments);
                }
            }

            return promise.Value;
        }

        public void SetInvocable(ValueTask value) =>
            promise = value;
    }
}
