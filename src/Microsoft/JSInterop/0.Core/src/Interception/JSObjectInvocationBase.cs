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
        public bool IsInterceptionStopped { get; private set; }

        public bool HasDeterminedResult =>
            AlternativeResult.HasValue;

        public bool HasAlternativeJSObjectReference =>
            !(alternativeJSObjectReference is null);

        public IJSObjectReference JSObjectReferenceResulting {
            get => alternativeJSObjectReference ?? JSObjectReferencePassed;
            set => alternativeJSObjectReference = value;
        }

        public IJSObjectReference JSObjectReferencePassed =>
            InvocationInception.JSObjectReference;

        public string JavaScriptIdentifier =>
            InvocationInception.JavaScriptIdentifier;

        public CancellationToken? JavaScriptCancellationToken =>
            InvocationInception.JavaScriptCancellationToken;

        public TimeSpan? Timeout =>
            InvocationInception.JavaScriptTimeout;

        public object?[] JavaScriptArguments =>
            InvocationInception.JavaScriptArguments;

        public virtual Type? TaskArgumentType =>
            InvocationInception.TaskArgumentType;

        public ICustomAttributes DefinitionAttributes =>
            InvocationInception.DefinitionAttributes;

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

        public void SetDeterminedResult(ReturnType value) =>
            AlternativeResult = value;

        public void StopInterception() =>
            IsInterceptionStopped = true;

        object IJSObjectInvocationBase.GetDeterminedResult() =>
             GetDeterminedResult();
    }
}
