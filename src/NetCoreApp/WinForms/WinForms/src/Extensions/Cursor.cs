using System.Drawing;
using System.Windows.Forms;

namespace Teronis.Extensions
{
    public static class CursorExtensions
    {
        public static Rectangle GetCursorRectangle(this Cursor cursor) =>
            new Rectangle(Cursor.Position, cursor.Size);
    }
}
