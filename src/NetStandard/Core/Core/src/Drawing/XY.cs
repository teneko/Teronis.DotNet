

namespace Teronis.Drawing
{
    public struct XY : IXY
    {
        public static readonly XY Empty = new XY();

        public int X { get; set; }
        public int Y { get; set; }

        public XY(int x, int y)
        {
            X = x;
            Y = y;
        }

        public XY(IXY xy) : this(xy.X, xy.Y) { }

        public bool Equals(IXY? other) => 
            XYEqualityComparer.Default.Equals(this, other);

        public override bool Equals(object? obj) => 
            Equals(obj as IXY);

        public override int GetHashCode() => XYEqualityComparer.Default.GetHashCode(this);
        public override string ToString() => XYEqualityComparer.Default.ToString(this);
    }
}
