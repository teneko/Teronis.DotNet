// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class CovariantKeyValuePairCollection<KeyType, ValueType> : IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>, ICovariantKeyValuePairCollection<KeyType, ValueType>
    {
        public int Count =>
            collection.Count;

        private readonly IReadOnlyCollection<KeyValuePair<KeyType, ValueType>> collection;

        public CovariantKeyValuePairCollection(IReadOnlyCollection<KeyValuePair<KeyType, ValueType>> collection) =>
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));

        IEnumerator IEnumerable.GetEnumerator() =>
            collection.GetEnumerator();

        IEnumerator<KeyValuePair<KeyType, ValueType>> IEnumerable<KeyValuePair<KeyType, ValueType>>.GetEnumerator() =>
            collection.GetEnumerator();

        public IEnumerator<ICovariantKeyValuePair<KeyType, ValueType>> GetEnumerator() =>
            new KeyValuePairEnumeratorWithPairAsCovariant<KeyType, ValueType>(collection.GetEnumerator());
    }
}
