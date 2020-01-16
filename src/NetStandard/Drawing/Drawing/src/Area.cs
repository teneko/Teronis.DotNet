using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Teronis.Extensions;

namespace Teronis.Drawing
{
    public class Area
    {
        /* TODO: reimplement */
        public static Area CreateArea([Optional]int? nX, [Optional] int? nY, [Optional] int? nWidth, [Optional] int? nHeight, [Optional] int? nYHeight, [Optional] int? nXWidth, [Optional] Rectangle? nRect, [Optional] bool nEllipse, [Optional] IList<Point> nPolygon, [Optional] bool precalculated)
        {
            return new Area() { nX = nX, nY = nY, nWidth = nWidth, nHeight = nHeight, nYHeight = nYHeight, nXWidth = nXWidth, nRect = nRect, nEllipse = nEllipse, nPolygon = nPolygon };
        }

        public static void VerifyValues(ISnapshot snapshot, ref int? nX, ref int? nY, ref int? nWidth, ref int? nHeight, RectangleF? nRect, bool nEllipse, IList<Point> nPolygon, ECancelScanIteration cancelScanIteration, ref bool precalculated, ref int? nYHeight, ref int? nXWidth)
        {
            if (!precalculated) {
                precalculated = true;

                if ((nX != null || nY != null || nWidth != null || nHeight != null) && (nRect != null || nPolygon != null)) { // see exception
                    throw new ArgumentException("Please just use nRect, nGraphPath OR one or more of nX, nY, nWidth and nHeight.");
                } else if (nPolygon != null && nEllipse)
                    throw new ArgumentException("It is not supported to extract a circle of a polygon.", nameof(nEllipse));
                else if ((nRect ?? (nRect = nPolygon?.GetBounds())) != null) { // fill base data
                    nX = (int)nRect.Value.X;
                    nY = (int)nRect.Value.Y;
                    nWidth = (int)nRect.Value.Width;
                    nHeight = (int)nRect.Value.Height;
                    nXWidth = nX + nWidth;
                    nYHeight = nY + nHeight;
                } else {
                    if (nWidth == null)
                        nWidth = nXWidth = snapshot.BitmapData.Rectangle.Width;

                    if (nHeight == null)
                        nHeight = nYHeight = snapshot.BitmapData.Rectangle.Height;

                    if (nX == null)
                        nX = 0;

                    if (nXWidth == null)
                        nXWidth = nX + nWidth;

                    if (nY == null)
                        nY = 0;

                    if (nYHeight == null)
                        nYHeight = nY + nHeight;
                }

                if (nX < 0 || nY < 0 || nXWidth > snapshot.BitmapData.Rectangle.Width || nYHeight > snapshot.BitmapData.Rectangle.Height) {
                    if (cancelScanIteration.HasFlag(ECancelScanIteration.OutOfIndex))
                        throw new IndexOutOfRangeException();
                    else {
                        if (nX < 0)
                            nX = 0;

                        if (nY < 0)
                            nY = 0;

                        if (nXWidth > snapshot.BitmapData.Rectangle.Width)
                            nXWidth -= nXWidth - snapshot.BitmapData.Rectangle.Width;

                        if (nYHeight > snapshot.BitmapData.Rectangle.Height)
                            nYHeight -= nYHeight - snapshot.BitmapData.Rectangle.Height;
                    }
                }
            }
        }

        public int? nX;
        public int? nY;
        public int? nWidth;
        public int? nHeight;
        public int? nXWidth;
        public int? nYHeight;
        public Rectangle? nRect;
        public bool nEllipse;
        public IList<Point> nPolygon;
        public bool precalculated;
        public ECancelScanIteration cancelScanIteration;

        private Area() { }

        public void VerifyValues(ISnapshot snapshot)
        {
            VerifyValues(snapshot, ref nX, ref nY, ref nWidth, ref nHeight, nRect, nEllipse, nPolygon, cancelScanIteration, ref precalculated, ref nYHeight, ref nXWidth);
        }
    }
}
