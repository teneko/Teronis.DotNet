// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Teronis.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Creates a rotated image from the image that got passed.
        /// </summary>
        /// <param name="image">The image to be rotated.</param>
        /// <param name="offset">The position to rotate from.</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees.</param>
        /// <returns>A new <see cref="Bitmap"/> of the same size rotated.</returns>
        /// <exception cref="ArgumentNullException">Thrown if image is null.</exception>
        public static Bitmap RotateImage(this Image image, PointF offset, float angle)
        {
            if (image == null) {
                throw new ArgumentNullException("image");
            }

            //create a new empty bitmap to hold rotated image
            var rotatedBmp = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            //make a graphics object from the empty bitmap
            var g = Graphics.FromImage(rotatedBmp);
            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);
            //rotate the image
            g.RotateTransform(angle);
            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);
            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));
            return rotatedBmp;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }
    }
}
