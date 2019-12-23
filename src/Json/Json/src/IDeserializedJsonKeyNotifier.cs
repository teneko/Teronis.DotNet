

namespace Teronis.Json
{
    public interface INotifyDeserializedJsonKey<KeyType>
    {
        void NotifyDeserializedJsonKey(KeyType key);
    }
}
