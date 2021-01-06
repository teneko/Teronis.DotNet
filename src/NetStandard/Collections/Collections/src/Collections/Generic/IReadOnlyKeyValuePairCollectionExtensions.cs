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
        public static CovariantKeyValuePairCollection<StillNullable<KeyType>, ValueType>? ToCovariantKeyValuePairCollectionWithNullableKeys<KeyType, ValueType>(this IReadOnlyCollection<KeyValuePair<StillNullable<KeyType>, ValueType>>? readOnlyCollection)
            where KeyType : notnull =>
            readOnlyCollection is null ? null : new CovariantKeyValuePairCollection<StillNullable<KeyType>, ValueType>(readOnlyCollection);
    }
}
