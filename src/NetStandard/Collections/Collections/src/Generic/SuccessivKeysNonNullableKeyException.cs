using System;

namespace Teronis.Collections.Generic
{
    public class SuccessivKeysNonNullableKeyException : ArgumentException
    {
        public int KeyIndex { get; }

        public SuccessivKeysNonNullableKeyException(int keyIndex) =>
            KeyIndex = keyIndex;

        public SuccessivKeysNonNullableKeyException(int keyIndex, string? message)
            : base(message) =>
            KeyIndex = keyIndex;

        public SuccessivKeysNonNullableKeyException(int keyIndex, string? message, Exception? innerException)
            : base(message, innerException) =>
            KeyIndex = keyIndex;

        public SuccessivKeysNonNullableKeyException(int keyIndex, string? message, string? paramName)
            : base(message, paramName) =>
            KeyIndex = keyIndex;

        public SuccessivKeysNonNullableKeyException(int keyIndex, string? message, string? paramName, Exception? innerException)
            : base(message, paramName, innerException) =>
            KeyIndex = keyIndex;
    }
}
