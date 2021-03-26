// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Teronis.Collections.Generic
{
    public class IEnumerableComparer<T> : EqualityComparer<IEnumerable<T>>
    {
        public static new IEnumerableComparer<T> Default { get; private set; }

        static IEnumerableComparer()
            => Default = new IEnumerableComparer<T>();

        public IEqualityComparer<T> EqualityComparer { get; private set; }

        public IEnumerableComparer(IEqualityComparer<T>? equalityComparer)
            => EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;

        public IEnumerableComparer()
            : this(null) { }

        public override bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y)) {
                return true;
            }

            if (x == null || y == null) {
                return false;
            }

            return x.SequenceEqual(y);
        }

        public override int GetHashCode(IEnumerable<T> obj)
        {
            // It will not throw an overflow exception.
            unchecked {
                return obj
                    .Select(e => e == null ? 0 : e.GetHashCode())
                    .Aggregate(17, (a, b) => 23 * a + b);
            }
        }
    }
}
