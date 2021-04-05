// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public abstract class JSObjectInvocationBase<ReturnType> : IJSObjectInvocationBase<ReturnType>
        where ReturnType : struct
    {
        public bool HasDeterminedResult =>
            AlternativeResult.HasValue;

        public bool HasAlternativeObjectReference =>
            !(alternativeJSObjectReference is null);

        public IJSObjectReference ObjectReference {
            get => alternativeJSObjectReference ?? OriginalObjectReference;
            set => alternativeJSObjectReference = value;
        }

        public IJSObjectReference OriginalObjectReference =>
            InvocationInception.JSObjectReference;

        public string Identifier =>
            InvocationInception.JavaScriptIdentifier;

        public CancellationToken? CancellationToken =>
            InvocationInception.JavaScriptCancellationToken;

        public TimeSpan? Timeout =>
            InvocationInception.JavaScriptTimeout;

        public object?[] Arguments =>
            InvocationInception.JavaScriptArguments;

        public virtual Type? TaskArgumentType =>
            InvocationInception.TaskArgumentType;

        public ICustomAttributes InvocationAttributes =>
            InvocationInception.DefinitionAttributes;

        public abstract IMemberDefinition InvocationDefinition { get; }

        internal ReturnType? AlternativeResult;

        internal readonly JSObjectInvocationInception<ReturnType> InvocationInception;

        private IJSObjectReference? alternativeJSObjectReference;

        internal JSObjectInvocationBase(
            JSObjectInvocationInception<ReturnType> invocationInception) =>
            InvocationInception = invocationInception;

        internal JSObjectInvocationBase(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            ICustomAttributes? memberAttributes)
        {
            InvocationInception = new JSObjectInvocationInception<ReturnType>(
                jSObjectReference,
                identifier,
                cancellationToken,
                timeout,
                arguments,
                memberAttributes);
        }

        internal JSObjectInvocationBase(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            ICustomAttributes? definitionAttributes,
            Type valueTaskGenericType)
        {
            InvocationInception = new JSObjectInvocationInception<ReturnType>(
                jSObjectReference,
                identifier,
                cancellationToken,
                timeout,
                arguments,
                definitionAttributes,
                valueTaskGenericType);
        }

        public abstract ReturnType GetDeterminedResult();

        public void SetAlternativeResult(ReturnType value) =>
            AlternativeResult = value;

        object IJSObjectInvocationBase.GetDeterminedResult() =>
             GetDeterminedResult();
    }
}
