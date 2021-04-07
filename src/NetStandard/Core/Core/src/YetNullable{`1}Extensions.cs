// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis
{
    public static class YetNullableExtensions
    {
        public static KeyType? ToNullable<KeyType>(this YetNullable<KeyType> nullableKey)
            where KeyType : struct
        {
            if (nullableKey.IsNull) {
                return null;
            }

            return nullableKey.Value;
        }
    }
}
