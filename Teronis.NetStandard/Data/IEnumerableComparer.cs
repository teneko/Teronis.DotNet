using System.Collections.Generic;
using System.Linq;

namespace Teronis.Data
{
    public class IEnumerableComparer<T> : EqualityComparer<IEnumerable<T>>
    {
        public static new IEnumerableComparer<T> Default { get; private set; }

        static IEnumerableComparer()
            => Default = new IEnumerableComparer<T>();

        public IEqualityComparer<T> ItemEqualityComparer { get; private set; }

        public IEnumerableComparer(IEqualityComparer<T> itemEqualityComparer)
            => ItemEqualityComparer = itemEqualityComparer ?? EqualityComparer<T>.Default;

        public IEnumerableComparer()
            : this(null) { }

        public override bool Equals(IEnumerable<T> x, IEnumerable<T> y)
            => ReferenceEquals(x, y) || (x != null && y != null && x.SequenceEqual(y));

        public override int GetHashCode(IEnumerable<T> obj)
        {
            // It will not throw an overflow exception
            unchecked
            {
                return obj
                    .Select(e => e == null ? 0 : e.GetHashCode())
                    .Aggregate(17, (a, b) => 23 * a + b);
            }
        }
    }
}
