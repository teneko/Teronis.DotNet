// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public abstract class JSFunctionalObjectInvocationBase<ReturnType> : IJSFunctionalObjectInvocationBase<ReturnType>
        where ReturnType : struct
    {
        public bool IsInterceptionStopped { get; private set; }

        /// <summary>
        /// Indicates whether the value task is set and can be awaited.
        /// </summary>
        public bool HasAlternativeResult =>
            AlternativeResult.HasValue;

        public bool HasAlternativeJSObjectReference =>
            !(alternativeJSObjectReference is null);

        /// <summary>
        /// The alternative JavaSript object reference or the original JavaScript
        /// object reference if alternative JavaSript object reference is null.
        /// </summary>
        public IJSObjectReference JSObjectReferenceResulting {
            get => alternativeJSObjectReference ?? JSObjectReferencePassed;
            set => alternativeJSObjectReference = value;
        }

        /// <summary>
        /// The JavaScript object reference that got initially passed.
        /// </summary>
        public IJSObjectReference JSObjectReferencePassed =>
            InvocationInception.JSObjectReference;

        public string Identifier =>
            InvocationInception.Identifier;

        public CancellationToken? CancellationToken =>
            InvocationInception.CancellationToken;

        public TimeSpan? Timeout =>
            InvocationInception.Timeout;

        public object?[] Arguments =>
            InvocationInception.Arguments;

        public virtual Type? GenericTaskArgumentType =>
            InvocationInception.GenericTaskArgumentType;

        internal ReturnType? AlternativeResult;

        internal readonly JSFunctionalObjectInvocationInception<ReturnType> InvocationInception;

        private IJSObjectReference? alternativeJSObjectReference;

        internal JSFunctionalObjectInvocationBase(
            JSFunctionalObjectInvocationInception<ReturnType> invocationInception)
        {
            InvocationInception = invocationInception;
        }

        internal JSFunctionalObjectInvocationBase(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments)
        {
            InvocationInception = new JSFunctionalObjectInvocationInception<ReturnType>(
                jSObjectReference,
                identifier,
                cancellationToken,
                timeout,
                arguments);
        }

        internal JSFunctionalObjectInvocationBase(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            Type valueTaskGenericType)
        {
            InvocationInception = new JSFunctionalObjectInvocationInception<ReturnType>(
                jSObjectReference,
                identifier,
                cancellationToken,
                timeout,
                arguments,
                valueTaskGenericType);
        }

        public abstract ReturnType GetResult();

        public void SetAlternativeResult(ReturnType value) =>
            AlternativeResult = value;

        public void StopInterception() =>
            IsInterceptionStopped = true;
    }
}
