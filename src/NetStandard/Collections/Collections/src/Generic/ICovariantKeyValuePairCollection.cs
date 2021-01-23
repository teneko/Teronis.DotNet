using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface ICovariantKeyValuePairCollection<out KeyType, out ValueType> : IEnumerable, IEnumerable<ICovariantKeyValuePair<KeyType, ValueType>>, IReadOnlyCollection<ICovariantKeyValuePair<KeyType, ValueType>>
    { }
}
