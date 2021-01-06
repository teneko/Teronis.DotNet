using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface IReadOnlyNullableKeyDictionary<KeyType, ValueType> : IReadOnlyDictionary<KeyType, ValueType>, IReadOnlyDictionary<StillNullable<KeyType>, ValueType>
        where KeyType : notnull
    {
        new int Count { get; }

        new ICollection<StillNullable<KeyType>> Keys { get; }
        new ICollection<ValueType> Values { get; }
    }
}
