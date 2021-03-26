// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace Teronis.Windows.Forms.Extensions
{
    public static class CursorExtensions
    {
        public static Rectangle GetCursorRectangle(this Cursor cursor) =>
            new Rectangle(Cursor.Position, cursor.Size);
    }
}
