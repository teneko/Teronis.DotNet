using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Drawing
{
    public struct XYZ : IXYZ
    {
        public static readonly XYZ Empty = new XYZ();

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public XYZ(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(IXY other) => XYEqualityComparer.Instance.Equals(this, other);
        public bool Equals(IXYZ other) => XYZEqualityComparer.Instance.Equals(this, other);

        public override bool Equals(object obj) => Equals(obj as IXYZ);
        public override int GetHashCode() => XYZEqualityComparer.Instance.GetHashCode(this);
        public override string ToString() => XYZEqualityComparer.Instance.ToString(this);
    }
}
