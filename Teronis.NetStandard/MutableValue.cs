using System;

namespace Teronis
{
    public class MutableValue<T> : IEquatable<T> where T : IEquatable<T>
    {
        public event EventHandler<T> ValueMutated;

        public T Value { get; private set; }

        public MutableValue(T value)
        {
            Value = value;
        }

        public void MutateValue(T mutation)
        {
            Value = mutation;
            ValueMutated?.Invoke(this, Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is MutableValue<T> mutation)
                return Value.Equals(mutation.Value);
            else if (obj is T val)
                return Value.Equals(val);
            else
                return false;
        }

        public override int GetHashCode() => Value.GetHashCode();
        public bool Equals(T other) => Value.Equals(other);
        public override string ToString() => Value.ToString();
    }
}
