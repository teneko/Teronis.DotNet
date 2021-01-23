using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Synchronization
{
    public class Number
    {
        public int Value { get; }
        public string? Tag { get; set; }
        public bool CompareValueWhenComparingReference { get; set; }

        public Number(int value) =>
            Value = value;

        public override bool Equals(object? obj) {
            if (obj is Number number) {
                return Value.Equals(number.Value);
            }

            return false;
        }

        public override int GetHashCode() =>
            Value.GetHashCode();

        public override string ToString() =>
            Value.ToString();

        public static implicit operator int(Number number) =>
            number.Value;

        public static implicit operator Number(int value) =>
           new Number(value);

        public class Comparer : Comparer<Number>
        {
            public new static Comparer Default => new Comparer();

            public override int Compare([AllowNull] Number x, [AllowNull] Number y)
            {
                if (ReferenceEquals(x, y)) {
                    return 0;
                }

                if (x is null && !(y is null)) {
                    return -1;
                }

                if (!(x is null) && y is null) {
                    return 1;
                }

                return x!.Value.CompareTo(y!.Value);
            }
        }

        public class ReferenceEqualityComparer : EqualityComparer<Number>
        {
            public new static ReferenceEqualityComparer Default = new ReferenceEqualityComparer();

            public override bool Equals([AllowNull] Number x, [AllowNull] Number y) =>
                ReferenceEquals(x, y);

            public override int GetHashCode([DisallowNull] Number obj) =>
                obj.GetHashCode();
        }

        public class ReferenceOrValueEqualityComparer : EqualityComparer<Number>
        {
            public new static ReferenceOrValueEqualityComparer Default = new ReferenceOrValueEqualityComparer();

            public override bool Equals([AllowNull] Number x, [AllowNull] Number y)
            {
                if (x is null && y is null) {
                    return false;
                }

                if (x is null || y is null) {
                    return false;
                }

                if (x.CompareValueWhenComparingReference || y.CompareValueWhenComparingReference) {
                    return x.Equals(y);
                }

                return ReferenceEquals(x, y);
            }

            public override int GetHashCode([DisallowNull] Number obj) =>
                obj.GetHashCode();
        }
    }
}
