// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public static class IValuableGenericExtensions
    {
        [return: MaybeNull]
        public static T GetValueOrDefault<T>(this IValuable<T> valuable) =>
            valuable.Value;

        [return: MaybeNull]
        public static T GetValueOrDefault<T>(this IValuable<T> valuable, T defaultValue) =>
            valuable.HasValue ? valuable.Value : defaultValue;
    }
}
