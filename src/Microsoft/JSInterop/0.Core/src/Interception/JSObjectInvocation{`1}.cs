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

        public ICustomAttributes TaskArgumentAttributes {
            get {
                if (taskArgumentTypeAttributes is null) {
                    taskArgumentTypeAttributes = new CustomAttributes(TaskArgumentType);
                }

                return taskArgumentTypeAttributes;
            }
        }

        public override IMemberDefinition InvocationDefinition {
            get {
                if (invocationDefinition is null) {
                    invocationDefinition = new InvocationDefinition(
                        Identifier,
                        TaskArgumentType,
                        InvocationAttributes,
                        new MemberTypeInfo(TaskArgumentType, TaskArgumentAttributes));
                }

                return invocationDefinition;
            }
        }

        private ICustomAttributes? taskArgumentTypeAttributes;
        private InvocationDefinition? invocationDefinition;

        internal JSObjectInvocation(JSObjectInvocationInception<ValueTask<TValue>> invocationInception)
            : base(invocationInception) { }

        internal JSObjectInvocation(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            ICustomAttributes? definitionAttributes)
            : base(
                  jSObjectReference,
                  identifier,
                  cancellationToken,
                  timeout,
                  arguments,
                  definitionAttributes,
                  typeof(TValue))
        { }

        public ValueTask<TNonDeterminedValue> GetNonDeterminedResult<TNonDeterminedValue>()
        {
            if (CancellationToken.HasValue) {
                return JSObjectReferenceExtensions.InvokeAsync<TNonDeterminedValue>(ObjectReference, Identifier, cancellationToken: CancellationToken.Value, Arguments);
            } else if (Timeout.HasValue) {
                return JSObjectReferenceExtensions.InvokeAsync<TNonDeterminedValue>(ObjectReference, Identifier, timeout: Timeout.Value, Arguments);
            } else {
                return JSObjectReferenceExtensions.InvokeAsync<TNonDeterminedValue>(ObjectReference, Identifier, Arguments);
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
                AlternativeResult = GetNonDeterminedResult<TValue>();
            }

            return AlternativeResult.Value;
        }

        public JSObjectInvocation<TValue> Clone() =>
             new JSObjectInvocation<TValue>(InvocationInception);

        IJSObjectInvocation<TValue> IJSObjectInvocation<TValue>.Clone() =>
            Clone();
    }
}
