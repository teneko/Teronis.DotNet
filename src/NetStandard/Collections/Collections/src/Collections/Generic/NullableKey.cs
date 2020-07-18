namespace Teronis.Collections.Generic
{
    public static class NullableKey
    {
        public static NullableKey<KeyType> Null<KeyType>()
            where KeyType : notnull =>
            NullableKey<KeyType>.Null;

        public static NullableKey<KeyType> FromNullable<KeyType>(KeyType? key)
            where KeyType : struct
        {
            if (key.HasValue) {
                return new NullableKey<KeyType>(key.Value);
            }

            return NullableKey<KeyType>.Null;
        }
    }
}
