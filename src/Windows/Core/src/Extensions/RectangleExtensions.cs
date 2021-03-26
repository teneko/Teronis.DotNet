// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;
using Teronis.Windows;

namespace Teronis.Windows.Extensions
{
    public static class RectangleExtensions
    {
        public static RECT GetRectangle(this Rectangle rectangle)
            => new RECT() { left = rectangle.X, top = rectangle.Y, right = rectangle.X + rectangle.Width, bottom = rectangle.Y + rectangle.Height };
    }
}
