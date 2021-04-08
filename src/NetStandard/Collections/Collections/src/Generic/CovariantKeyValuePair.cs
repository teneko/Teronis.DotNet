// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Generic
{
    public static class CovariantKeyValuePair
    {
        public static CovariantKeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
            new CovariantKeyValuePair<TKey, TValue>(key, value);
    }
}
