// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public static class INullableExtensions
    {
        public static bool TryGetNotNull<T>(this INullable<T> nullable, [MaybeNullWhen(false)] out T value)
        {
            if (!(nullable.Value is null)) {
                value = nullable.Value;
                return true;
            }

            value = default;
            return false;
        }

        public static bool TryGetNull<T>(this INullable<T> nullable, [MaybeNullWhen(true)] out T value)
        {
            if (nullable.Value is null) {
                value = default;
                return true;
            }

            value = nullable.Value;
            return false;
        }
    }
}
