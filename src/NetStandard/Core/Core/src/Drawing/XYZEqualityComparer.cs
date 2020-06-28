using System.Collections.Generic;

namespace Teronis.Drawing
{
    public class XYZEqualityComparer : EqualityComparer<IXYZ>
    {
        public new static XYZEqualityComparer Default { get; } = new XYZEqualityComparer();

        public override bool Equals(IXYZ? x, IXYZ? y)
        {
            if (x == null && y == null) {
                return true;
            }

            if (x == null || y == null) {
                return false;
            }

            if (XYEqualityComparer.Default.Equals(x, y) && x.Z == y.Z) {
                return true;
            }

            return false;
        }

        public override int GetHashCode(IXYZ obj)
        {
            unchecked {
                var hash = XYEqualityComparer.Default.GetHashCode(obj);
                hash = hash * 23 + obj.Z.GetHashCode();
                return hash;
            }
        }

        public string ToString(IXYZ xyz) => $"{xyz.X}:{xyz.Y}:{xyz.Z}";
    }
}
