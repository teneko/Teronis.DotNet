// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public static class IYetNullableExtensions
    {
        public static bool TryGetNotNull<T>(this IYetNullable<T> nullable, [MaybeNullWhen(false)] out T value)
        {
            if (nullable.IsNotNull) {
                value = nullable.Value!;
                return true;
            }

            value = default;
            return false;
        }
    }
}
