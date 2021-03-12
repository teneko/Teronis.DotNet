using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public ref struct JSFunctionalObjectInvocation<TValue>
    {
        /// <summary>
        /// Indicates whether the value task is set and can be awaited.
        /// </summary>
        public bool IsPromiseRedeemable =>
            promise.HasValue;

        public bool IsUsingAlternativeJSObjectReference =>
            !(alternativeJSObjectReference is null);

        /// <summary>
        /// The alternative JavaSript object reference or the original JavaScript
        /// object reference if alternative JavaSript object reference is null.
        /// </summary>
        public IJSObjectReference JSObjectReference {
            get => alternativeJSObjectReference ?? OriginalJSObjectReference;
            set => alternativeJSObjectReference = value;
        }
        /// <summary>
        /// The JavaScript object reference that got initially passed.
        /// </summary>
        public readonly IJSObjectReference OriginalJSObjectReference { get; }
        public readonly string Identifier { get; }
        public readonly CancellationToken? CancellationToken { get; }
        public readonly TimeSpan? Timeout { get; }
        public readonly object?[] Arguments { get; }
        public readonly Type GenericTaskArgumentType { get; }

        private ValueTask<TValue>? promise;
        private IJSObjectReference? alternativeJSObjectReference;

        internal JSFunctionalObjectInvocation(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments)
        {
            OriginalJSObjectReference = jSObjectReference;
            Identifier = identifier;
            CancellationToken = cancellationToken;
            Timeout = timeout;
            Arguments = arguments;
            GenericTaskArgumentType = typeof(TValue);
            promise = null;
            alternativeJSObjectReference = null;
        }

        /// <summary>
        /// Sets the promise. 
        /// </summary>
        /// <param name="value"></param>
        public void SetPromise(ValueTask<TValue> value) =>
            promise = value;

        /// <summary>
        /// If promise does not exist the promise will be the result of one of 
        /// the extension methods of <see cref="JSObjectReferenceExtensions"/>
        /// depending on <see cref="CancellationToken"/> and 
        /// <see cref="Timeout"/>. The result will be cached.
        /// </summary>
        /// <returns>The promise.</returns>
        public ValueTask<TValue> GetPromise()
        {
            if (!promise.HasValue) {
                if (CancellationToken.HasValue) {
                    promise = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReference, Identifier, cancellationToken: CancellationToken.Value, Arguments);
                } else if (Timeout.HasValue) {
                    promise = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReference, Identifier, timeout: Timeout.Value, Arguments);
                } else {
                    promise = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReference, Identifier, Arguments);
                }
            }

            return promise.Value;
        }
    }
}
