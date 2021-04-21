// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public static class ICovariantKeyValuePairExtensions
    {
        public static void Deconstruct<KeyType, ValueType>(this ICovariantKeyValuePair<KeyType, ValueType> pair, out KeyType key, [MaybeNull] out ValueType value)
        {
            key = pair.Key;
            value = pair.Value;
        }

        public static ICovariantKeyValuePair<KeyType, ValueType> AsCovariant<KeyType, ValueType>(this ICovariantKeyValuePair<KeyType, ValueType> pair) =>
            pair;
    }
}
