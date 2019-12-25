
namespace Teronis.Collections.Generic
{
    public struct ValueIndexPair<T>
    {
        public T Value { get; private set; }
        public int Index { get; private set; }

        public ValueIndexPair(T value, int index)
        {
            Value = value;
            Index = index;
        }

        public override string ToString() => $"[{Value}, {Index}]";
    }
}
