// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;
using System.Drawing.Imaging;

namespace Teronis.Windows.Drawing
{
    public unsafe class BitmapData : IBitmapData
    {
        public byte* ScreenData { get; private set; }
        public int Stride => bitmapData.Stride;
        public Rectangle Rectangle { get; private set; }

        readonly Bitmap bmap;
        readonly System.Drawing.Imaging.BitmapData bitmapData;

        public BitmapData(Bitmap bmap)
        {
            this.bmap = bmap;
            Rectangle = new Rectangle(Point.Empty, bmap.Size);
            bitmapData = bmap.LockBits(Rectangle, ImageLockMode.ReadWrite, bmap.PixelFormat);
            ScreenData = (byte*)bitmapData.Scan0.ToPointer();
        }

        public BitmapData(Bitmap bmap, out BitmapShot bmapShot) : this(bmap)
            => bmapShot = new BitmapShot(this);

        public void Dispose() => bmap.UnlockBits(bitmapData);
    }
}
