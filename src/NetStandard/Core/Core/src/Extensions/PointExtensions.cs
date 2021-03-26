// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;

namespace Teronis.Extensions
{
    public static class PointExtensions
    {
        public static bool Compare(this Point point, int x, int y)
        {
            return point.X == x && point.Y == y;
        }

        public static bool Compare(this Point point1, Point point2)
        {
            return point1.Compare(point2.X, point2.Y);
        }

        public static Point GetOffset(this Point point, Point offset)
        {
            point.Offset(offset);
            return point;
        }
    }
}
