// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class RectangleExtensions
    {
        public static bool IsInEllipse(this Rectangle rectangle, int x, int y) => RectangleUtils.IsRectangleInEllipse(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, x, y);

        public static bool Contains(this Rectangle rectangle, int x, int y, bool ellipse) => !ellipse ? rectangle.Contains(x, y) : rectangle.IsInEllipse(x, y);
    }
}
