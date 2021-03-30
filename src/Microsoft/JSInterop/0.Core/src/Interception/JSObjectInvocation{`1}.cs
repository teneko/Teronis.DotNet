// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSObjectInvocation<TValue> : JSObjectInvocationBase<ValueTask<TValue>>, IJSObjectInvocation<TValue>
    {
        public override Type TaskArgumentType =>
            base.TaskArgumentType!;

        public ICustomAttributes TaskArgumentTypeAttributes {
            get {
                if (taskArgumentTypeAttributes is null) {
                    taskArgumentTypeAttributes = new CustomAttributeLookup(TaskArgumentType);
                }

                return taskArgumentTypeAttributes;
            }
        }

        private ICustomAttributes? taskArgumentTypeAttributes;

        internal JSObjectInvocation(JSObjectInvocationInception<ValueTask<TValue>> invocationInception)
            : base(invocationInception) { }

        internal JSObjectInvocation(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            ICustomAttributes memberAttributes)
            : base(
                  jSObjectReference,
                  identifier,
                  cancellationToken,
                  timeout,
                  arguments,
                  memberAttributes,
                  typeof(TValue))
        { }

        public ValueTask<TNonDeterminingValue> GetNonDeterminingResult<TNonDeterminingValue>()
        {
            if (JavaScriptCancellationToken.HasValue) {
                return JSObjectReferenceExtensions.InvokeAsync<TNonDeterminingValue>(JSObjectReferenceResulting, JavaScriptIdentifier, cancellationToken: JavaScriptCancellationToken.Value, JavaScriptArguments);
            } else if (Timeout.HasValue) {
                return JSObjectReferenceExtensions.InvokeAsync<TNonDeterminingValue>(JSObjectReferenceResulting, JavaScriptIdentifier, timeout: Timeout.Value, JavaScriptArguments);
            } else {
                return JSObjectReferenceExtensions.InvokeAsync<TNonDeterminingValue>(JSObjectReferenceResulting, JavaScriptIdentifier, JavaScriptArguments);
            }
        }

        /// <summary>
        /// If promise does not exist the promise will be the result of one of 
        /// the extension methods of <see cref="JSObjectReferenceExtensions"/>
        /// depending on <see cref="CancellationToken"/> and 
        /// <see cref="Timeout"/>. The result will be cached.
        /// </summary>
        /// <returns>The promise.</returns>
        public override ValueTask<TValue> GetDeterminedResult()
        {
            if (!AlternativeResult.HasValue) {
                AlternativeResult = GetNonDeterminingResult<TValue>();
            }

            return AlternativeResult.Value;
        }

        public JSObjectInvocation<TValue> Clone() =>
             new JSObjectInvocation<TValue>(InvocationInception);

        IJSObjectInvocation<TValue> IJSObjectInvocation<TValue>.Clone() =>
            Clone();
    }
}
