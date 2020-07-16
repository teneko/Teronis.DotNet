namespace Teronis.Collections.Generic
{
    public static class NullableKeyExntensions
    {
        public static KeyType? ToNullable<KeyType>(this NullableKey<KeyType> nullableKey)
            where KeyType : struct
        {
            if (nullableKey.IsNull) {
                return null;
            }

            return nullableKey.Key;
        }
    }
}
