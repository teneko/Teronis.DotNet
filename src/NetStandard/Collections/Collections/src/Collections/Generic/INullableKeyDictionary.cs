using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public interface INullableKeyDictionary<KeyType, ValueType> : IDictionary<KeyType, ValueType>, IDictionary<NullableKey<KeyType>, ValueType>
        where KeyType : notnull
    { }
}
