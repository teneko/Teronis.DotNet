using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public interface ICovariantKeyValuePair<out KeyType, out ValueType>
    {
        KeyType Key { get; }
        [MaybeNull]
        ValueType Value { get; }
    }
}
