using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public static class ICovariantKeyValuePairExtensions
    {
        public static void Deconstruct<KeyType, ValueType>(this ICovariantKeyValuePair<KeyType, ValueType> pair, out KeyType key, [MaybeNull] out ValueType value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
