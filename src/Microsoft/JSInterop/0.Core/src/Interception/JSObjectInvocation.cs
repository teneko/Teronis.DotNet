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

        public ValueTask GetNonDeterminingResult()
        {
            if (JavaScriptCancellationToken.HasValue) {
                return JSObjectReferenceResulting.InvokeVoidAsync(JavaScriptIdentifier, cancellationToken: JavaScriptCancellationToken.Value, JavaScriptArguments);
            } else if (Timeout.HasValue) {
                return JSObjectReferenceResulting.InvokeVoidAsync(JavaScriptIdentifier, timeout: Timeout.Value, JavaScriptArguments);
            } else {
                return JSObjectReferenceResulting.InvokeVoidAsync(JavaScriptIdentifier, JavaScriptArguments);
            }
        }

        public override ValueTask GetDeterminedResult()
        {
            if (!AlternativeResult.HasValue) {
                AlternativeResult = GetNonDeterminingResult();
            }

            return AlternativeResult.Value;
        }

        public JSObjectInvocation Clone() =>
            new JSObjectInvocation(InvocationInception);

        IJSObjectInvocation IJSObjectInvocation.Clone() =>
           Clone();
    }
}
