using System.Collections.Generic;

namespace Teronis.Extensions.Collections
{
    public static class ICollectionGenericExtensions
    {
        public static ICollection<KeyValuePair<KeyType, ValueType>> AsCollectionWithPairs<KeyType, ValueType>(this ICollection<KeyValuePair<KeyType, ValueType>> collection) =>
            collection;
    }
}
