namespace Teronis
{
    public static class YetNullableExtensions
    {
        public static KeyType? ToNullable<KeyType>(this YetNullable<KeyType> nullableKey)
            where KeyType : struct
        {
            if (!nullableKey.HasValue) {
                return null;
            }

            return nullableKey.Value;
        }
    }
}
