// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Interception
{
    internal readonly struct JSFunctionalObjectInvocationInception<ReturnType>
    {
        public readonly IJSObjectReference JSObjectReference { get; }
        public readonly string Identifier { get; }
        public readonly CancellationToken? CancellationToken { get; }
        public readonly TimeSpan? Timeout { get; }
        public readonly object?[] Arguments { get; }
        public readonly Type? GenericTaskArgumentType { get; }

        public JSFunctionalObjectInvocationInception(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments,
            Type valueTaskGenericArgument)
        {
            JSObjectReference = jSObjectReference ?? throw new ArgumentNullException(nameof(jSObjectReference));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            CancellationToken = cancellationToken;
            Timeout = timeout;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            GenericTaskArgumentType = valueTaskGenericArgument ?? throw new ArgumentNullException(nameof(valueTaskGenericArgument));
        }

        public JSFunctionalObjectInvocationInception(
            IJSObjectReference jSObjectReference,
            string identifier,
            CancellationToken? cancellationToken,
            TimeSpan? timeout,
            object?[] arguments)
        {
            JSObjectReference = jSObjectReference ?? throw new ArgumentNullException(nameof(jSObjectReference));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            CancellationToken = cancellationToken;
            Timeout = timeout;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            GenericTaskArgumentType = null;
        }
    }
}
