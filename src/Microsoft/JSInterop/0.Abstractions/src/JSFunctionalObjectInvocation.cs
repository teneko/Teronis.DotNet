using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectInvocation : JSFunctionalObjectInvocationBase<ValueTask>, IJSFunctionalObjectInvocation
    {
        internal JSFunctionalObjectInvocation(JSFunctionalObjectInvocationInception<ValueTask> invocationInception)
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
                  arguments)
        { }

        public override ValueTask GetResult()
        {
            if (!AlternativeResult.HasValue) {
                if (CancellationToken.HasValue) {
                    AlternativeResult = JSObjectReferenceExtensions.InvokeVoidAsync(JSObjectReferenceResulting, Identifier, cancellationToken: CancellationToken.Value, Arguments);
                    ;
                } else if (Timeout.HasValue) {
                    AlternativeResult = JSObjectReferenceExtensions.InvokeVoidAsync(JSObjectReferenceResulting, Identifier, timeout: Timeout.Value, Arguments);
                } else {
                    AlternativeResult = JSObjectReferenceExtensions.InvokeVoidAsync(JSObjectReferenceResulting, Identifier, Arguments);
                }
            }

            return AlternativeResult.Value;
        }

        void IJSFunctionalObjectInvocation.Clone() =>
            new JSFunctionalObjectInvocation(InvocationInception);
    }
}
