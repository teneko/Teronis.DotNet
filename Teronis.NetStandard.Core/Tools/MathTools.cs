using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Teronis.NetStandard.Tools
{
    public static class MathTools
    {
        public static int GetZeroBasedModulo(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static int GetOneBasedModulo(int x, int m)
        {
            int r = x % m;
            return r <= 0 ? r + m : r;
        }

        public static PointF GetAveragePoint<T>(IList<T> points, Func<T, Point> getPoint)
        {
            float avgX = getPoint(points[0]).X, avgY = getPoint(points[0]).Y, count = points.Count;

            for (var i = 1; i < count; i++) {
                var pointObject = points[i];
                var point = getPoint(pointObject);
                avgX += point.X;
                avgY += point.Y;
            }

            avgX /= count;
            avgY /= count;

            return new PointF(avgX, avgY);
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

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            return new Point {
                X = (int)(cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y = (int)(sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        public static double GetAngelBetweenTwoPoints(PointF p1, PointF p2)
        {
            //// angle in radians
            //return Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
            // angle in degrees
            return (Math.Atan2(p1.Y - p2.Y, p1.X - p2.X) * 180 / Math.PI) + 180; // -180-180 -> 0-360
        }

        public static double DistanceBetweenTwoPoints(PointF p1, PointF p2)
        {
            float deltaX = p2.X - p1.X;
            float deltaY = p2.Y - p1.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static List<T> GetCirclePoints<T>(ICollection pointObjects, Func<T, PointF> retPoint, PointF centerMiddlePoint, int radius)
        {
            var posisGroup = new List<T>();

            foreach (var pointObject in pointObjects) {
                var point = (T)pointObject;

                if (DistanceBetweenTwoPoints(retPoint(point), centerMiddlePoint) < radius)
                    posisGroup.Add(point);
            }

            return posisGroup;
        }

        public static IEnumerable<Point> GetCirclePoints(float width, float height, float middleCenterX, float middleCenterY, float radius, bool border = false)
        {
            middleCenterX -= 0.5f;
            middleCenterY -= 0.5f;

            var outerRadiusSquared = radius * radius;
            var innerRadiusSquared = Math.Pow(radius - 1, 2);

            float yEnd = middleCenterY + radius;
            if (yEnd > height)
                yEnd = height - 1;

            float xEnd = middleCenterX + radius;
            if (xEnd > width)
                xEnd = width - 1;

            for (int y = 0; y <= yEnd; y++) {
                for (int x = 0; x <= xEnd; x++) {
                    var dx = x - middleCenterX;
                    var dy = y - middleCenterY;
                    var distanceSquared = dx * dx + dy * dy;

                    if (distanceSquared <= outerRadiusSquared && (border ? distanceSquared > innerRadiusSquared : true))
                        yield return new Point(x, y);
                }
            }
        }

        public static bool IsPointInCircle(float middleCenterX, float middleCenterY, float radius, float x, float y)
        {
            middleCenterX = ((middleCenterX * 2) - 1) / 2;
            middleCenterY = ((middleCenterY * 2) - 1) / 2;
            var outerRadiusSquared = radius * radius;
            var dx = x - middleCenterX;
            var dy = y - middleCenterY;
            var distanceSquared = dx * dx + dy * dy;
            return distanceSquared <= outerRadiusSquared;
        }

        public static bool IsPointInEllipse(float middleCenterX, float middleCenterY, float radiusWidth, float radiusHeight, int x, int y)
        {
            middleCenterX -= 0.5f;
            middleCenterY -= 0.5f;
            //
            return Math.Pow(x - middleCenterX, 2) / Math.Pow(radiusWidth, 2) + Math.Pow(y - middleCenterY, 2) / Math.Pow(radiusHeight, 2) <= 1;
        }

        public static bool IsRectangleInEllipse(int rectangleX, int rectangleY, int rectangleWidth, int rectangleHeight, int x, int y)
        {
            var relMiddleCenterX = rectangleWidth / 2f;
            var relMiddleCenterY = rectangleHeight / 2f;
            var absMiddleCenterX = rectangleX + relMiddleCenterX;
            var absMiddleCenterY = rectangleY + relMiddleCenterY;
            //
            return IsPointInEllipse((float)absMiddleCenterX, (float)absMiddleCenterY, (float)relMiddleCenterX, (float)relMiddleCenterY, x, y);
        }

        public static bool RectangleContains(int rectangleX, int rectnalgeY, int rectangleWidth, int rectangleHeight, int x, int y)
        {
            return rectangleX <= x &&
                x < rectangleX + rectangleWidth &&
                rectnalgeY <= y &&
                y < rectnalgeY + rectangleHeight;
        }

        /// <summary>
        /// Decision between Rcetangle.Contains() and "Ellipse.Contains()";
        /// </summary>
        /// <param name="rectangleX"></param>
        /// <param name="rectangleY"></param>
        /// <param name="rectangleWidth"></param>
        /// <param name="rectangleHeight"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="inEllipse"></param>
        /// <returns></returns>
        public static bool RectangleContains(int rectangleX, int rectangleY, int rectangleWidth, int rectangleHeight, int x, int y, bool inEllipse) => (inEllipse ? IsRectangleInEllipse(rectangleX, rectangleY, rectangleWidth, rectangleHeight, x, y) : RectangleContains(rectangleX, rectangleY, rectangleWidth, rectangleHeight, x, y));
    }
}
