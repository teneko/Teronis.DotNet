using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Teronis.NetStandard.Tools
{
    public static class PointTools
    {
        /// <summary>
        /// Creates a rectangle based on two points.
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Rectangle</returns>
        public static RectangleF GetRectangle(Point p1, Point p2)
        {
            int top = Math.Min(p1.Y, p2.Y);
            int right = Math.Max(p1.X, p2.X);
            int bottom = Math.Max(p1.Y, p2.Y);
            int left = Math.Min(p1.X, p2.X);
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        /// <summary>
        /// Creates a rectangle based on two points.
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Rectangle</returns>
        public static RectangleF GetRectangle(PointF p1, PointF p2)
        {
            float top = Math.Min(p1.Y, p2.Y);
            float right = Math.Max(p1.X, p2.X);
            float bottom = Math.Max(p1.Y, p2.Y);
            float left = Math.Min(p1.X, p2.X);
            return RectangleF.FromLTRB(left, top, right, bottom);
        }
    }
}
