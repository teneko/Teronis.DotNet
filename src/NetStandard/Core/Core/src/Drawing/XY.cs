// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Drawing
{
    public readonly struct XY : IXY
    {
        public static readonly XY Empty = new XY();

        public readonly int X { get; }
        public readonly int Y { get; }

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

        public override int GetHashCode() =>
            XYEqualityComparer.Default.GetHashCode(this);

        public override string ToString() =>
            $"{X}:{Y}";
    }
}
