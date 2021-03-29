// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    internal readonly struct JSObjectInvocationInception<ReturnType>
    {
        public readonly IJSObjectReference JSObjectReference { get; }
        public readonly string JavaScriptIdentifier { get; }
        public readonly CancellationToken? JavaScriptCancellationToken { get; }
        public readonly TimeSpan? JavaScriptTimeout { get; }
        public readonly object?[] JavaScriptArguments { get; }
        public readonly Type? GenericTaskArgumentType { get; }
        public readonly ICustomAttributes MemberAttributes { get; }

        public JSObjectInvocationInception(
            IJSObjectReference jSObjectReference,
            string javaScriptIdentifier,
            CancellationToken? javaScriptCancellationToken,
            TimeSpan? javaScriptTimeout,
            object?[] javaScriptArguments,
            ICustomAttributes memberAttributes,
            Type genericTaskArgumentType)
        {
            JSObjectReference = jSObjectReference ?? throw new ArgumentNullException(nameof(jSObjectReference));
            JavaScriptIdentifier = javaScriptIdentifier ?? throw new ArgumentNullException(nameof(javaScriptIdentifier));
            JavaScriptCancellationToken = javaScriptCancellationToken;
            JavaScriptTimeout = javaScriptTimeout;
            JavaScriptArguments = javaScriptArguments ?? throw new ArgumentNullException(nameof(javaScriptArguments));
            GenericTaskArgumentType = genericTaskArgumentType ?? throw new ArgumentNullException(nameof(genericTaskArgumentType));
            MemberAttributes = memberAttributes ?? throw new ArgumentNullException(nameof(memberAttributes));
        }

        public JSObjectInvocationInception(
            IJSObjectReference jSObjectReference,
            string javaScriptIdentifier,
            CancellationToken? javaScriptCancellationToken,
            TimeSpan? javaScriptTimeout,
            object?[] javaScriptArguments,
            ICustomAttributes methodAttributes)
        {
            JSObjectReference = jSObjectReference ?? throw new ArgumentNullException(nameof(jSObjectReference));
            JavaScriptIdentifier = javaScriptIdentifier ?? throw new ArgumentNullException(nameof(javaScriptIdentifier));
            JavaScriptCancellationToken = javaScriptCancellationToken;
            JavaScriptTimeout = javaScriptTimeout;
            JavaScriptArguments = javaScriptArguments ?? throw new ArgumentNullException(nameof(javaScriptArguments));
            GenericTaskArgumentType = null;
            MemberAttributes = methodAttributes ?? throw new ArgumentNullException(nameof(methodAttributes));
        }
    }
}
