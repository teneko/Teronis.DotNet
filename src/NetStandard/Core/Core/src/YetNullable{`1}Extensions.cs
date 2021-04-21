// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis
{
    public static class YetNullableExtensions
    {
        public static KeyType? ToNullable<KeyType>(this YetNullable<KeyType> nullableValue)
            where KeyType : struct
        {
            if (nullableValue.IsNull) {
                return null;
            }

            return nullableValue.Value;
        }

        public static IYetNullable<TKey> AsCovariant<TKey>(this YetNullable<TKey> nullableValue) =>
            nullableValue;
    }
}
