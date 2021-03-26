// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Interception
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
                    AlternativeResult = JSObjectReferenceResulting.InvokeVoidAsync(Identifier, cancellationToken: CancellationToken.Value, Arguments);
                    ;
                } else if (Timeout.HasValue) {
                    AlternativeResult = JSObjectReferenceResulting.InvokeVoidAsync(Identifier, timeout: Timeout.Value, Arguments);
                } else {
                    AlternativeResult = JSObjectReferenceResulting.InvokeVoidAsync(Identifier, Arguments);
                }
            }

            return AlternativeResult.Value;
        }

        void IJSFunctionalObjectInvocation.Clone() =>
            new JSFunctionalObjectInvocation(InvocationInception);
    }
}
