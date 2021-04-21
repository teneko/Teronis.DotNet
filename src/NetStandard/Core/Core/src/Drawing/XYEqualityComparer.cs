// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Drawing
{
    public class XYEqualityComparer : EqualityComparer<IXY>
    {
        public new static XYEqualityComparer Default { get; } = new XYEqualityComparer();

        public override bool Equals(IXY? x, IXY? y)
        {
            if (x == null && y == null) {
                return true;
            }

            if (x == null || y == null) {
                return false;
            }

            if (x.X == y.X && x.Y == y.Y) {
                return true;
            }

            return false;
        }

        public override int GetHashCode(IXY obj) =>
            HashCode.Combine(obj.X, obj.Y);
    }
}
