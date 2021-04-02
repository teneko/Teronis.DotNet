// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSObjectInvocation : JSObjectInvocationBase<ValueTask>, IJSObjectInvocation
    {
        internal JSObjectInvocation(JSObjectInvocationInception<ValueTask> invocationInception)
               : base(invocationInception) { }

        internal JSObjectInvocation(
            IJSObjectReference jsObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            ICustomAttributes? memberAttributes)
            : base(
                  jsObjectReference,
                  identifier,
                  cancellationToken,
                  timeout,
                  arguments,
                  memberAttributes)
        { }

        public ValueTask GetNonDeterminedResult()
        {
            if (CancellationToken.HasValue) {
                return ObjectReference.InvokeVoidAsync(Identifier, cancellationToken: CancellationToken.Value, Arguments);
            } else if (Timeout.HasValue) {
                return ObjectReference.InvokeVoidAsync(Identifier, timeout: Timeout.Value, Arguments);
            } else {
                return ObjectReference.InvokeVoidAsync(Identifier, Arguments);
            }
        }

        public override ValueTask GetDeterminedResult()
        {
            if (!AlternativeResult.HasValue) {
                AlternativeResult = GetNonDeterminedResult();
            }

            return AlternativeResult.Value;
        }

        public JSObjectInvocation Clone() =>
            new JSObjectInvocation(InvocationInception);

        IJSObjectInvocation IJSObjectInvocation.Clone() =>
           Clone();
    }
}
