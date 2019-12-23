using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teronis.NetStandard.Extensions
{
    public static class BitmapExtensions
    {
        public static Rectangle GetRectangle(this Bitmap bmap) => new Rectangle(Point.Empty, bmap.Size);
    }
}
