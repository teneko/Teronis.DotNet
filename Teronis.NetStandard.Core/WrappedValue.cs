using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public class WrappedValue<T>
    {
        public T Value { get; set; }

        public WrappedValue(T value) => Value = value;

        public static implicit operator WrappedValue<T>(T value) => new WrappedValue<T>(value);
    }
}
