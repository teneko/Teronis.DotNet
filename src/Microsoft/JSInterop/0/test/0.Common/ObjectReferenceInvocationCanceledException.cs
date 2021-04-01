// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;
using System.Threading;

namespace Teronis.Microsoft.JSInterop
{
    public class ObjectReferenceInvocationCanceledException : OperationCanceledException
    {
        public string JavaScriptIdentifier { get; private set; } = null!;
        public object?[]? JavaScriptArguments { get; private set; }

        public ObjectReferenceInvocationCanceledException(string jsIdentifier, object?[]? jsArguments) =>
            onInitialize(jsIdentifier, jsArguments);

        public ObjectReferenceInvocationCanceledException(string jsIdentifier, object?[]? jsArguments, string? message)
            : base(message) =>
            onInitialize(jsIdentifier, jsArguments);

        public ObjectReferenceInvocationCanceledException(string jsIdentifier, object?[]? jsArguments, CancellationToken token)
            : base(token) =>
            onInitialize(jsIdentifier, jsArguments);

        public ObjectReferenceInvocationCanceledException(string jsIdentifier, object?[]? jsArguments, string? message, Exception? innerException)
            : base(message, innerException) =>
            onInitialize(jsIdentifier, jsArguments);

        public ObjectReferenceInvocationCanceledException(string jsIdentifier, object?[]? jsArguments, string? message, CancellationToken token)
            : base(message, token) =>
            onInitialize(jsIdentifier, jsArguments);

        public ObjectReferenceInvocationCanceledException(string jsIdentifier, object?[]? jsArguments, string? message, Exception? innerException, CancellationToken token)
            : base(message, innerException, token) =>
            onInitialize(jsIdentifier, jsArguments);

        protected ObjectReferenceInvocationCanceledException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        private void onInitialize(string jsIdentifier, object?[]? jsArguments)
        {
            JavaScriptIdentifier = jsIdentifier ?? throw new ArgumentNullException(nameof(jsIdentifier));
            JavaScriptArguments = jsArguments;
        }
    }
}
