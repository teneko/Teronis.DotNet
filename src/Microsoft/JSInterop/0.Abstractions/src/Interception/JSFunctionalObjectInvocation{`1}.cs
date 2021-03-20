using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSFunctionalObjectInvocation<TValue> : JSFunctionalObjectInvocationBase<ValueTask<TValue>>, IJSFunctionalObjectInvocation<TValue>
    {
        public override Type GenericTaskArgumentType =>
            base.GenericTaskArgumentType!;

        internal JSFunctionalObjectInvocation(JSFunctionalObjectInvocationInception<ValueTask<TValue>> invocationInception)
            : base(invocationInception) { }

        internal JSFunctionalObjectInvocation(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments)
            : base(
                  jSObjectReference,
                  identifier,
                  cancellationToken,
                  timeout,
                  arguments,
                  typeof(TValue))
        { }

        /// <summary>
        /// If promise does not exist the promise will be the result of one of 
        /// the extension methods of <see cref="JSObjectReferenceExtensions"/>
        /// depending on <see cref="CancellationToken"/> and 
        /// <see cref="Timeout"/>. The result will be cached.
        /// </summary>
        /// <returns>The promise.</returns>
        public override ValueTask<TValue> GetResult()
        {
            if (!AlternativeResult.HasValue) {
                if (CancellationToken.HasValue) {
                    AlternativeResult = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReferenceResulting, Identifier, cancellationToken: CancellationToken.Value, Arguments);
                } else if (Timeout.HasValue) {
                    AlternativeResult = JSObjectReferenceResulting.InvokeAsync<TValue>(Identifier, timeout: Timeout.Value, Arguments);
                } else {
                    AlternativeResult = JSObjectReferenceExtensions.InvokeAsync<TValue>(JSObjectReferenceResulting, Identifier, Arguments);
                }
            }

            return AlternativeResult.Value;
        }

        void IJSFunctionalObjectInvocation<TValue>.Clone() =>
            new JSFunctionalObjectInvocation<TValue>(InvocationInception);
    }
}
