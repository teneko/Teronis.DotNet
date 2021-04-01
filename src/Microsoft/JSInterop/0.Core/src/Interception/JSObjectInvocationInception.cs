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
        public readonly Type? TaskArgumentType { get; }
        public readonly ICustomAttributes DefinitionAttributes { get; }

        public JSObjectInvocationInception(
            IJSObjectReference jSObjectReference,
            string javaScriptIdentifier,
            CancellationToken? javaScriptCancellationToken,
            TimeSpan? javaScriptTimeout,
            object?[] javaScriptArguments,
            ICustomAttributes? definitionAttributes,
            Type taskArgumentType)
        {
            JSObjectReference = jSObjectReference ?? throw new ArgumentNullException(nameof(jSObjectReference));
            JavaScriptIdentifier = javaScriptIdentifier ?? throw new ArgumentNullException(nameof(javaScriptIdentifier));
            JavaScriptCancellationToken = javaScriptCancellationToken;
            JavaScriptTimeout = javaScriptTimeout;
            JavaScriptArguments = javaScriptArguments ?? throw new ArgumentNullException(nameof(javaScriptArguments));
            TaskArgumentType = taskArgumentType ?? throw new ArgumentNullException(nameof(taskArgumentType));
            DefinitionAttributes = definitionAttributes ?? CustomAttributes.Empty;
        }

        public JSObjectInvocationInception(
            IJSObjectReference jSObjectReference,
            string javaScriptIdentifier,
            CancellationToken? javaScriptCancellationToken,
            TimeSpan? javaScriptTimeout,
            object?[] javaScriptArguments,
            ICustomAttributes? definitionAttributes)
        {
            JSObjectReference = jSObjectReference ?? throw new ArgumentNullException(nameof(jSObjectReference));
            JavaScriptIdentifier = javaScriptIdentifier ?? throw new ArgumentNullException(nameof(javaScriptIdentifier));
            JavaScriptCancellationToken = javaScriptCancellationToken;
            JavaScriptTimeout = javaScriptTimeout;
            JavaScriptArguments = javaScriptArguments ?? throw new ArgumentNullException(nameof(javaScriptArguments));
            TaskArgumentType = null;
            DefinitionAttributes = definitionAttributes ?? CustomAttributes.Empty;
        }
    }
}
