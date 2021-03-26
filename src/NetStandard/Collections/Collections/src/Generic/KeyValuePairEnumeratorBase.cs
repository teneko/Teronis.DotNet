// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public abstract class KeyValuePairEnumeratorBase<CurrentType, KeyType, ValueType> : IEnumerator<KeyValuePair<KeyType, ValueType>>
    {
        private readonly IEnumerator<KeyValuePair<KeyType, ValueType>> enumerator;

        [AllowNull, MaybeNull]
        public CurrentType Current { get; private set; } = default!;

        object? IEnumerator.Current => enumerator.Current;
        KeyValuePair<KeyType, ValueType> IEnumerator<KeyValuePair<KeyType, ValueType>>.Current => enumerator.Current;

        public KeyValuePairEnumeratorBase(IEnumerator<KeyValuePair<KeyType, ValueType>> enumerator) =>
            this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

        protected abstract CurrentType CreateCurrent(KeyValuePair<KeyType, ValueType> currentPair);

        public void Dispose() =>
            enumerator.Dispose();

        public bool MoveNext()
        {
            if (enumerator.MoveNext()) {
                Current = CreateCurrent(enumerator.Current);
                return true;
            }

            return false;
        }

        public void Reset() =>
            enumerator.Reset();
    }
}
