// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using Teronis.Extensions;
using Teronis.Windows;

namespace Teronis.Windows.Extensions
{
    public static class IListPointExtensions
    {
        public static Rectangle GetBounds(this IList<Point> points)
        {
            if (points == null || points.Count == 0) {
                throw new Exception();
            }

            var rect = new RECT() { left = points[0].X, top = points[0].Y, right = points[0].X, bottom = points[0].Y };

            foreach (var point in points) {
                if (point.X < rect.left) {
                    rect.left = point.X;
                } else if (point.X > rect.right) {
                    rect.right = point.X;
                }

                if (point.Y < rect.top) {
                    rect.top = point.Y;
                } else if (point.Y > rect.bottom) {
                    rect.bottom = point.Y;
                }
            }

            return rect.GetRectangle();
        }
    }
}
