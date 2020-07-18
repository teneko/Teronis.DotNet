using System;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface INullableKeyDictionary<KeyType, ValueType> : IDictionary<KeyType, ValueType>, IDictionary<NullableKey<KeyType>, ValueType>
        where KeyType : notnull
    {
        /// <summary>
        /// Adds an element associated with the nullable key.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">Null key exists already.</exception>
        /// <exception cref="NotSupportedException">Dictionary is read-only.</exception>
        void Add(ValueType value);
    }
}
