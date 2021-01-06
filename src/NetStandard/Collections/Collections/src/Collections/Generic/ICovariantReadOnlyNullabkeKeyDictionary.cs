using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface ICovariantReadOnlyNullabkeKeyDictionary<KeyType, out ValueType> :
        ICovariantReadOnlyDictionary<KeyType, ValueType>, ICovariantReadOnlyDictionary<StillNullable<KeyType>, ValueType>,
        IEnumerable
        where KeyType : notnull
    {
        new IEnumerable<KeyType> Keys { get; }
        new IEnumerable<ValueType> Values { get; }
        new int Count { get; }
    }
}
