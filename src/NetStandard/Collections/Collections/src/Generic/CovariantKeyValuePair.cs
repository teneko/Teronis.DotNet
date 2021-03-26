// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Generic
{
    public static class CovariantKeyValuePair
    {
        public static CovariantKeyValuePair<KeyType, ValueType> Create<KeyType, ValueType>(KeyType key, ValueType value) =>
            new CovariantKeyValuePair<KeyType, ValueType>(key, value);
    }
}
