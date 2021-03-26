// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Teronis.Utils
{
    public static class PointUtils
    {
        public static IEnumerable<Point> GetCirclePoints(float width, float height, float middleCenterX, float middleCenterY, float radius, bool border = false)
        {
            middleCenterX -= 0.5f;
            middleCenterY -= 0.5f;

            var outerRadiusSquared = radius * radius;
            var innerRadiusSquared = Math.Pow(radius - 1, 2);

            float yEnd = middleCenterY + radius;
            if (yEnd > height) {
                yEnd = height - 1;
            }

            float xEnd = middleCenterX + radius;
            if (xEnd > width) {
                xEnd = width - 1;
            }

            for (int y = 0; y <= yEnd; y++) {
                for (int x = 0; x <= xEnd; x++) {
                    var dx = x - middleCenterX;
                    var dy = y - middleCenterY;
                    var distanceSquared = dx * dx + dy * dy;

                    if (distanceSquared <= outerRadiusSquared && (!border || distanceSquared > innerRadiusSquared)) {
                        yield return new Point(x, y);
                    }
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
    }
}
