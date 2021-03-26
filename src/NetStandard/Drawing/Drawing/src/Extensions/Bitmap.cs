// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;

namespace Teronis.Extensions
{
    public static class BitmapExtensions
    {
        public static Rectangle GetRectangle(this Bitmap bmap) => new Rectangle(Point.Empty, bmap.Size);
    }
}
