using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Teronis.NetStandard.Drawing
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name=""image"">The <see cref=""System.Drawing.Image"/"> to rotate
        /// <param name=""offset"">The position to rotate from.
        /// <param name=""angle"">The amount to rotate the image, clockwise, in degrees
        /// <returns>A new <see cref=""System.Drawing.Bitmap"/"> of the same size rotated.</see>
        /// <exception cref=""System.ArgumentNullException"">Thrown if <see cref=""image"/"> 
        /// is null.</see>
        public static Bitmap RotateImage(this Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

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
    }
}
