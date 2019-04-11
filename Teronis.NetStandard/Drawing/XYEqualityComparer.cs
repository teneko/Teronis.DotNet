using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Drawing
{
    public class XYEqualityComparer : EqualityComparer<IXY>
    {
        public static XYEqualityComparer Instance { get; } = new XYEqualityComparer();

        public override bool Equals(IXY x, IXY y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x.X == y.X && x.Y == y.Y)
                return true;
            else
                return false;
        }

        public override int GetHashCode(IXY obj)
        {
            unchecked {
                var hash = 17;
                hash = hash * 23 + obj.X.GetHashCode();
                hash = hash * 23 + obj.Y.GetHashCode();
                return hash;
            }
        }

        public string ToString(IXY xy) => $"{xy.X}:{xy.Y}";
    }
}
