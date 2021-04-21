// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Drawing
{
    public struct XYZ : IXYZ
    {
        public static readonly XYZ Empty = new XYZ();

        public readonly int X { get; }
        public readonly int Y { get; }
        public readonly int Z { get; }

        public XYZ(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(IXY? other) =>
            XYEqualityComparer.Default.Equals(this, other);

        public bool Equals(IXYZ? other) =>
            XYZEqualityComparer.Default.Equals(this, other);

        public override bool Equals(object? obj) =>
            Equals(obj as IXYZ);

        public override int GetHashCode() =>
            XYZEqualityComparer.Default.GetHashCode(this);

        public override string ToString() =>
            $"{X}:{Y}:{Z}";
    }
}
