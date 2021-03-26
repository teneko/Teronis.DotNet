// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public abstract class KeyValuePairEnumeratorWithConversionBase<CurrentType, KeyType, ValueType> : KeyValuePairEnumeratorBase<CurrentType, KeyType, ValueType>, IEnumerator<CurrentType>
    {
        CurrentType IEnumerator<CurrentType>.Current => Current!;

        public KeyValuePairEnumeratorWithConversionBase(IEnumerator<KeyValuePair<KeyType, ValueType>> enumerator)
            : base(enumerator) { }
    }
}
