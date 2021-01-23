namespace Teronis.Json.Converters
{
    /// <summary>
    /// If implemented on a class that is used as JSON object value, it will announce its property name from its origin JSON object that wouldn't be available otherwise.
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    public interface IAnnounceJsonPropertyName<KeyType>
    {
        void AnnouncePropertyName(KeyType key);
    }
}
