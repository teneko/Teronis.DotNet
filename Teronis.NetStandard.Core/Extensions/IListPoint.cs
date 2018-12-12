using System;
using System.Collections.Generic;
using System.Drawing;
using Teronis.NetStandard.Windows;

namespace Teronis.NetStandard.Extensions
{
    public static class IListPointExtensions
    {
        public static Rectangle GetBounds(this IList<Point> points)
        {
            if (points == null || points.Count == 0)
                throw new Exception();

            var rect = new RECT() { left = points[0].X, top = points[0].Y, right = points[0].X, bottom = points[0].Y };

            foreach (var point in points) {
                if (point.X < rect.left)
                    rect.left = point.X;
                else if (point.X > rect.right)
                    rect.right = point.X;

                if (point.Y < rect.top)
                    rect.top = point.Y;
                else if (point.Y > rect.bottom)
                    rect.bottom = point.Y;
            }

            return rect.GetRectangle();
        }

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
