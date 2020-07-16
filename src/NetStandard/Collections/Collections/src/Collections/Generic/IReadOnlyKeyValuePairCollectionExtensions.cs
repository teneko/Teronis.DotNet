using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public static class IReadOnlyKeyValuePairCollectionExtensions
    {
        [return: NotNullIfNotNull("readOnlyCollection")]
        public static CovariantKeyValuePairCollection<KeyType, ValueType>? ToCovariantKeyValuePairCollection<KeyType, ValueType>(this IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>? readOnlyCollection)
            where KeyType : notnull =>
            readOnlyCollection is null ? null : new CovariantKeyValuePairCollection<KeyType, ValueType>(readOnlyCollection);

        [return: NotNullIfNotNull("readOnlyCollection")]
        public static CovariantKeyValuePairCollection<NullableKey<KeyType>, ValueType>? ToCovariantKeyValuePairCollectionWithNullableKeys<KeyType, ValueType>(this IReadOnlyCollection<KeyValuePair<NullableKey<KeyType>, ValueType>>? readOnlyCollection)
            where KeyType : notnull =>
            readOnlyCollection is null ? null : new CovariantKeyValuePairCollection<NullableKey<KeyType>, ValueType>(readOnlyCollection);
    }
}
