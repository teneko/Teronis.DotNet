namespace Teronis.Collections.Generic
{
    public static class CovariantKeyValuePairCollectionExtensions
    {
        public static ICovariantKeyValuePairCollection<KeyType, ValueType> AsCovariantKeyValuePairCollection<KeyType, ValueType>(this CovariantKeyValuePairCollection<KeyType, ValueType> collection)
            where KeyType : notnull =>
            collection;
    }
}
