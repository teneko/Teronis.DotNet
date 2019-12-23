

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

        public bool Equals(IXY other) => XYEqualityComparer.Default.Equals(this, other);
        public bool Equals(IXYZ other) => XYZEqualityComparer.Default.Equals(this, other);

        public override bool Equals(object obj) => Equals(obj as IXYZ);
        public override int GetHashCode() => XYZEqualityComparer.Default.GetHashCode(this);
        public override string ToString() => XYZEqualityComparer.Default.ToString(this);
    }
}
