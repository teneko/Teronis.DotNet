// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Collections.Generic
{
    public class SuccessiveKeysNonNullableKeyException : ArgumentException
    {
        public int KeyIndex { get; }

        public SuccessiveKeysNonNullableKeyException(int keyIndex) =>
            KeyIndex = keyIndex;

        public SuccessiveKeysNonNullableKeyException(int keyIndex, string? message)
            : base(message) =>
            KeyIndex = keyIndex;

        public SuccessiveKeysNonNullableKeyException(int keyIndex, string? message, Exception? innerException)
            : base(message, innerException) =>
            KeyIndex = keyIndex;

        public SuccessiveKeysNonNullableKeyException(int keyIndex, string? message, string? paramName)
            : base(message, paramName) =>
            KeyIndex = keyIndex;

        public SuccessiveKeysNonNullableKeyException(int keyIndex, string? message, string? paramName, Exception? innerException)
            : base(message, paramName, innerException) =>
            KeyIndex = keyIndex;
    }
}
