using System.Drawing;

namespace Teronis.Extensions
{
    public static class BitmapExtensions
    {
        public static Rectangle GetRectangle(this Bitmap bmap) => new Rectangle(Point.Empty, bmap.Size);
    }
}
