

namespace Teronis.Data
{
    public class WrappedValue<T>
    {
        public virtual T Value { get; set; }

        public WrappedValue(T value) => Value = value;

        public virtual T GetValue() => Value;

        public static implicit operator WrappedValue<T>(T value) => new WrappedValue<T>(value);
    }
}
