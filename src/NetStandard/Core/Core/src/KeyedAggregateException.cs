// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Teronis
{
    public class KeyedAggregateException : AggregateException
    {
        protected KeyedAggregateException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public KeyedAggregateException() =>
            setInnerExceptionsWithKeys();

        public KeyedAggregateException(string? message)
            : base(message) =>
            setInnerExceptionsWithKeys();

        public KeyedAggregateException(string? message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            errorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
            innerException = innerException ?? throw new ArgumentNullException(nameof(innerException));

            var keyedExceptions = new Dictionary<string, Exception>() {
                {  errorCode, innerException }
            };

            setInnerExceptionsWithKeys(keyedExceptions);
        }

        public KeyedAggregateException(string? message, IDictionary<string, Exception> keyedExceptions)
            : base(message, keyedExceptions?.Values ?? throw new ArgumentNullException(nameof(keyedExceptions))) =>
            setInnerExceptionsWithKeys(keyedExceptions);

        private void setInnerExceptionsWithKeys(IDictionary<string, Exception>? keyedExceptions = null)
        {
            keyedExceptions = keyedExceptions ?? new Dictionary<string, Exception>();
            InnerExceptionsWithKeys = new ReadOnlyDictionary<string, Exception>(keyedExceptions);
        }

        public ReadOnlyDictionary<string, Exception> InnerExceptionsWithKeys { get; private set; } = null!;
    }
}
