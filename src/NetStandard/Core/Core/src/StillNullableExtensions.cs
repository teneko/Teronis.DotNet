namespace Teronis
{
    public static class StillNullableExtensions
    {
        public static KeyType? ToNullable<KeyType>(this StillNullable<KeyType> nullableKey)
            where KeyType : struct
        {
            if (!nullableKey.HasValue) {
                return null;
            }

            return nullableKey.Value;
        }
    }
}
