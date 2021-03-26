// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class KeyValuePairEnumeratorWithPairAsCovariant<KeyType, ValueType> : KeyValuePairEnumeratorWithConversionBase<ICovariantKeyValuePair<KeyType, ValueType>, KeyType, ValueType>
    {
        public KeyValuePairEnumeratorWithPairAsCovariant(IEnumerator<KeyValuePair<KeyType, ValueType>> enumerator)
            : base(enumerator)
        { }

        protected override ICovariantKeyValuePair<KeyType, ValueType> CreateCurrent(KeyValuePair<KeyType, ValueType> currentPair) =>
            new CovariantKeyValuePair<KeyType, ValueType>(currentPair.Key, currentPair.Value);
    }
}
