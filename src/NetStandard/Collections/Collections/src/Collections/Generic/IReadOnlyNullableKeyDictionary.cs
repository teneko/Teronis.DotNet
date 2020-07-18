using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface IReadOnlyNullableKeyDictionary<KeyType, ValueType> : IReadOnlyDictionary<KeyType, ValueType>, IReadOnlyDictionary<NullableKey<KeyType>, ValueType>
        where KeyType : notnull
    { }
}
