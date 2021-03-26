// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Teronis.Utils
{
    public static class PointFUtils
    {
        public static double GetAngelBetweenTwoPoints(PointF p1, PointF p2)
        {
            //// angle in radians
            //return Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
            // angle in degrees
            return (Math.Atan2(p1.Y - p2.Y, p1.X - p2.X) * 180 / Math.PI) + 180; // -180-180 -> 0-360
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

        /// <summary>
        /// Rotates one point around another.
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static PointF RotatePoint(PointF pointToRotate, PointF centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            return new PointF {
                X = (int)(cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y = (int)(sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        /// <summary>
        /// Gets a new point in a given circle.
        /// </summary>
        public static PointF GetNewPointInLine(PointF location, double radius, double degrees)
        {
            var x = location.X + (radius * Math.Cos(degrees * Math.PI / 180));
            var y = location.Y + (radius * Math.Sin(degrees * Math.PI / 180));
            return new PointF((float)x, (float)y);
        }

        public static double GetDistanceBetweenTwoPoints(PointF p1, PointF p2)
        {
            float deltaX = p2.X - p1.X;
            float deltaY = p2.Y - p1.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static PointF GetAveragePoint(IEnumerable<PointF> points)
        {
            var result = default(PointF);

            if (points != null) {
                var enumerator = points.GetEnumerator();

                // At least one point.
                if (enumerator.MoveNext()) {
                    var counter = 0;

                    do {
                        result.X += enumerator.Current.X;
                        result.Y += enumerator.Current.Y;
                        counter++;
                    } while (enumerator.MoveNext());

                    result.X /= counter;
                    result.Y /= counter;
                }
            }

            return result;
        }

        public static PointF GetAveragePoint<T>(IEnumerable<T> points, Func<T, PointF> getPoint)
        {
            IEnumerable<PointF> getPoints()
            {
                foreach (var point in points) {
                    yield return getPoint(point);
                }
            }

            return GetAveragePoint(getPoints());
        }

        public static IEnumerable<PointF> GetCirclePoints(IEnumerable<PointF> points, PointF centerMiddlePoint, int radius)
        {
            foreach (var point in points) {
                if (GetDistanceBetweenTwoPoints(point, centerMiddlePoint) < radius) {
                    yield return point;
                }
            }
        }

        public static IEnumerable<T> GetCirclePoints<T>(IEnumerable<T> points, Func<T, PointF> getPoint, PointF centerMiddlePoint, int radius)
        {
            foreach (var point in points) {
                if (GetDistanceBetweenTwoPoints(getPoint(point), centerMiddlePoint) < radius) {
                    yield return point;
                }
            }
        }
    }
}
