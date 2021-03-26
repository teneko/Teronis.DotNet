// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;

namespace Teronis.Extensions
{
    public static class IListPointExtensions
    {
        public static bool IsInPolygon(this IList<Point> points, Point p)
        {
            int i, j = points.Count - 1;
            bool isVisible = false;
            for (i = 0; i < points.Count; i++) {
                if (points[i].Y < p.Y && points[j].Y >= p.Y
                     || points[j].Y < p.Y && points[i].Y >= p.Y) {
                    if (points[i].X + (float)(p.Y - points[i].Y) / (points[j].Y - points[i].Y)
                         * (points[j].X - points[i].X) < p.X) {
                        isVisible = !isVisible;
                    }
                }
                j = i;
            }
            return isVisible;
        }
    }
}
