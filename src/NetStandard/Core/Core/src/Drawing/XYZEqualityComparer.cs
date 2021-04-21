// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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

        public override int GetHashCode(IXYZ obj) =>
            HashCode.Combine(obj.X, obj.Y, obj.Z);
    }
}
