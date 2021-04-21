// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public static class SuccessiveKeys
    {
        [return: MaybeNull]
        public static KeyType Retype<KeyType>(object? key)
        {
            if (key is null) {
                var type = typeof(KeyType);

                if (!type.IsValueType || Nullable.GetUnderlyingType(type) != null) {
                    return default;
                }

                throw new ArgumentException("One key of successive keys is null, but the type of that key is not nullable.");
            }

            return (KeyType)key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="KeyType"></typeparam>
        /// <param name="keys"></param>
        /// <param name="index"></param>
        /// <returns>Retyped key.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Key index is out of range.</exception>
        /// <exception cref="SuccessiveKeysNonNullableKeyException">Key type is not nullable although key is null.</exception>
        [return: MaybeNull]
        public static KeyType Retype<KeyType>(object?[] keys, int index)
        {
            if (index < 0 || index >= keys.Length) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            try {
                return Retype<KeyType>(keys[index]);
            } catch (ArgumentException) {
                throw new SuccessiveKeysNonNullableKeyException(index, $"Key at index {index} of one array is null, but the type of that key is not nullable.");
            }
        }
    }
}
