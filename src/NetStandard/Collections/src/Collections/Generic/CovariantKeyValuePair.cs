namespace Teronis.Collections.Generic
{
    public static class CovariantKeyValuePair
    {
        public static CovariantKeyValuePair<KeyType, ValueType> Create<KeyType, ValueType>(KeyType key, ValueType value) =>
            new CovariantKeyValuePair<KeyType, ValueType>(key, value);
    }
}
