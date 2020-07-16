namespace Teronis.Collections.Generic
{
    public interface ICovariantKeyValuePair<out KeyType, out ValueType>
    {
        KeyType Key { get; }
        ValueType Value { get; }
    }
}
