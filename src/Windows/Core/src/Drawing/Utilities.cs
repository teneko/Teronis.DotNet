// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;

namespace Teronis.Windows.Drawing
{
    public static class Utilities
    {
        public static Bitmap CaptureWindow(IntPtr hWnd)
        {
            var rctForm = Rectangle.Empty;
            using (var grfx = Graphics.FromHdc(Win32.GetWindowDC(hWnd))) {
                rctForm = Rectangle.Round(grfx.VisibleClipBounds);
            }

            var pImage = new Bitmap(rctForm.Width, rctForm.Height);
            var graphics = Graphics.FromImage(pImage);
            var hDC = graphics.GetHdc();
            //paint control onto graphics using provided options        
            try {
                Win32.PrintWindow(hWnd, hDC, 0);
            } finally {
                graphics.ReleaseHdc(hDC);
            }
            return pImage;
        }
    }
}

