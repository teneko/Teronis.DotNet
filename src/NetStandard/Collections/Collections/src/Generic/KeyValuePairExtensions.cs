// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    internal static class KeyValuePairExtensions
    {
        public static void Deconstruct<KeyType, ValueType>(this KeyValuePair<KeyType, ValueType> pair, out KeyType key, out ValueType value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
