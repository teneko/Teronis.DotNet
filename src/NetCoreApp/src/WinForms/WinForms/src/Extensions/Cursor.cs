using System.Drawing;
using System.Windows.Forms;

namespace Teronis.Extensions
{
    public static class CursorExtensions
    {
        public static Rectangle GetCursorRectangle(this Cursor cursor)
        {
            return new Rectangle(Cursor.Position, Cursor.Current.Size);
        }
    }
}
