

namespace Teronis
{
    public class ValueWrap<T>
    {
        public virtual T Value { get; set; }

        public ValueWrap(T value) => Value = value;

        public virtual T GetValue() => Value;

        public static implicit operator ValueWrap<T>(T value) => new ValueWrap<T>(value);
    }
}
