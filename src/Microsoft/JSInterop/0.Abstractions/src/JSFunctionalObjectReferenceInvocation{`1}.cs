using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public ref struct JSFunctionalObjectReferenceInvocation<TValue>
    {
        public bool CanInvoke => promise.HasValue;

        public readonly IJSObjectReference JSObjectReference { get; }
        public readonly string Identifier { get; }
        public readonly CancellationToken? CancellationToken { get; }
        public readonly TimeSpan? TimeSpan { get; }
        public readonly object?[] Arguments { get; }
        public readonly Type GenericTaskArgumentType { get; }

        private ValueTask<TValue>? promise;

        internal JSFunctionalObjectReferenceInvocation(
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
            GenericTaskArgumentType = typeof(TValue);
            promise = null;
        }

        public ValueTask<TValue> InvokeAsync()
        {
            if (!promise.HasValue) {
                if (CancellationToken.HasValue) {
                    promise = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReference, Identifier, cancellationToken: CancellationToken.Value, Arguments);
                } else if (TimeSpan.HasValue) {
                    promise = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReference, Identifier, timeout: TimeSpan.Value, Arguments);
                } else {
                    promise = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReference, Identifier, Arguments);
                }
            }

            return promise.Value;
        }

        public void SetInvocable(ValueTask<TValue> value) =>
            promise = value;
    }
}
