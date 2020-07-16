namespace Teronis.Collections.Generic
{
    public interface INullableKey<out KeyType>
        where KeyType : notnull
    {
        bool IsNull { get; }
        KeyType Key { get; }
    }
}
