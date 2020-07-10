using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using Force.Crc32;
using Teronis.Extensions;
using Teronis.Tools;

namespace Teronis.Drawing
{
    public unsafe class BitmapShot : ISnapshot
    {
        public IBitmapData BitmapData { get; private set; }

        public BitmapShot(IBitmapData bitmapData) => BitmapData = bitmapData;

        public delegate IEnumerable<Pixel> DelegateIteratePixelsAny([Optional] int? nX, [Optional] int? nY, [Optional] int? nWidth, [Optional] int? nHeight, [Optional] int? nYHeight, [Optional] int? nXWidth, [Optional] Rectangle? nRect, [Optional] bool nCircle, [Optional] IList<Point> nPolygon, [Optional] List<ITwoDimensionalPattern> twoDPatterns, [Optional] int? maxPixelAmount, [Optional] bool crop, [Optional] IEnumerable<RGBColor> colors, [Optional] bool negateColorResult, [Optional] IEnumerable<ColorTranslation> translateColor, [Optional] IEnumerable<Area> excludedAreas, [Optional] bool precalculated, [Optional] bool debug, [Optional] ECancelScanIteration cancelScanIteration);

        /// <summary>Any passed points here (<paramref name="nX"/>, <paramref name="nY"/>, ...) must be passed from the top left corner of the window that has been passed by handle (<see cref="WindowHandle"/>) previously.</summary>
        /// <param name="nX">Fill one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/> OR ONLY <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/>.</param>
        /// <param name="nY">Fill one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/> OR ONLY <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/>.</param>
        /// <param name="nWidth">Fill one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/> OR ONLY <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/>.</param>
        /// <param name="nHeight">Fill one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/> OR ONLY <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/>.</param>
        /// <param name="nYHeight">DO NOT SET THIS. It is set by sub call. It represents the vertical maximum (Y+Height).</param>
        /// <param name="nXWidth">Do NOT SET THIS. It is set by sub call. It represents the horizontal maximum (X+Width)</param>
        /// <param name="nRect">Fill only <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/> OR one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/></param>
        /// <param name="nEllipse">Fill only <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/> OR one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/></param>
        /// <param name="nPolygon">Fill only <paramref name="nRect"/>, <paramref name="nEllipse"/> or <paramref name="nPolygon"/> OR one or more of <paramref name="nX"/>,<paramref name="nY"/>, <paramref name="nWidth"/> and <paramref name="nHeight"/></param>
        /// <param name="twoDPatterns">You can search for pattern. Each pixel iterates this list.</param>
        /// <param name="crop">Positions are changed absolute to nX, nY. When found pixel is at x=20 and nX=20 then x becomes x=0</param>
        /// <param name="colors">You are able to search for colors as much as you want.</param>
        /// <param name="negateColorResult">Those pixel colors that has been found are not found and vice versa.</param>
        /// <param name="translateColorList">This is kind of a post production. After a pixel color is same as one of the colors list, then it will be 'translated'. It won't get sorted out - post production.</param>
        /// <param name="excludedAreas">Within your defined area, you can exclude areas.</param>
        /// <param name="precalculated">DO NOT SET THIS. It is set by sub call. Passed values will be get proved and some values, such as nXWidth and nYHeight, get calculated.</param>
        /// <param name="debug">Not implemented right now.</param>
        /// <param name="cancelScanIteration">When set true and you ususally would get an error because you passed to big area you are getting an IndexOutOfRange excpetion you can catch. Otherwise the error is getting ignored and you receive zero pixels.</param>
        /// <returns>Returns an IEnumerable you can loop. The items are returned by yield.</returns>
        public IEnumerable<Pixel> IteratePixelsAny([Optional] int? nX, [Optional] int? nY, [Optional] int? nWidth, [Optional] int? nHeight, [Optional] int? nYHeight, [Optional] int? nXWidth, [Optional] Rectangle? nRect, [Optional] bool nEllipse, [Optional] IList<Point> nPolygon, [Optional] List<ITwoDimensionalPattern> twoDPatterns, [Optional] int? maxPixelAmount, [Optional] bool crop, [Optional] IEnumerable<RGBColor> colors, [Optional] bool negateColorResult, [Optional] IEnumerable<ColorTranslation> translateColorList, [Optional] IEnumerable<Area> excludedAreas, [Optional] bool precalculated, [Optional] bool debug, [Optional] ECancelScanIteration cancelScanIteration)
        {
            try {
                Area.VerifyValues(this, ref nX, ref nY, ref nWidth, ref nHeight, nRect, nEllipse, nPolygon, cancelScanIteration, ref precalculated, ref nYHeight, ref nXWidth);
            } catch (IndexOutOfRangeException) {
#if DEBUG
                "Wanted exception by argument 'cancelScanIteration'!".ToConsole();
#endif
                goto exit;
            }

            var debugDict = new Dictionary<uint, string>();
            var pixelCounter = 0;

            if (twoDPatterns != null && twoDPatterns.Count > 0) {
                twoDPatterns.Add(null);
            } else if (twoDPatterns != null && twoDPatterns.Count == 0) {
                twoDPatterns = null;
            }

            for (var lineY = (int)nY; lineY < nYHeight; lineY++) {
                for (var lineX = (int)nX; lineX < nXWidth; lineX++) {
                    if (maxPixelAmount != null && pixelCounter == maxPixelAmount) {
                        goto exit;
                    }

                    var point = new Point(lineX, lineY);

                    getClrBytes(lineX, lineY, out byte r, out byte g, out byte b, out byte _);

                    if (debug) {
                        debugDict[Crc32CAlgorithm.Compute(new byte[] { r, g, b })] = $"{point.X}/{point.Y} -> {r} {g} {b}";
                    }

                    if (colors != null) {
                        foreach (var color in colors) {
                            if (color.Compare(r, g, b)) {
                                if (negateColorResult) {
                                    goto @continue;
                                } else {
                                    goto success;
                                }
                            }
                        }

                        if (negateColorResult) {
                            goto success;
                        }

                        goto @continue;

                        success:
                        ;
                    }

                    if (translateColorList != null) {
                        foreach (var translateColorItem in translateColorList) {
                            if (translateColorItem.FromColor.Compare(r, g, b)) {
                                r = translateColorItem.ToColor.R;
                                g = translateColorItem.ToColor.G;
                                b = translateColorItem.ToColor.B;
                            }
                        }
                    }

                    if (nPolygon != null && !nPolygon.IsInPolygon(point)) // sort out points that are not in polygon/ellipse
{
                        continue;
                    } else if (nEllipse && !RectangleTools.IsRectangleInEllipse((int)nX, (int)nY, (int)nWidth, (int)nHeight, lineX, lineY)) {
                        continue;
                    }

                    if (excludedAreas != null) {
                        foreach (var area in excludedAreas) {
                            area.VerifyValues(this);

                            if ((area.nPolygon != null ? area.nPolygon.IsInPolygon(point) : RectangleTools.RectangleContains((int)area.nX, (int)area.nY, (int)area.nWidth, (int)area.nHeight, lineX, lineY, area.nEllipse))) {
                                goto @continue;
                            }
                        }
                    }

                    if (twoDPatterns != null) {
                        int twoDPatternsIndex = 0;

                        for (twoDPatternsIndex = 0; twoDPatternsIndex < twoDPatterns.Count; twoDPatternsIndex++) {
                            if (twoDPatternsIndex == twoDPatterns.Count - 1) {
                                goto @continue;
                            }

                            var pattern = twoDPatterns[twoDPatternsIndex];
                            pattern.GetPosition(out var pos);
                            pos.X += lineX;
                            pos.Y += lineY;

                            if (RectangleTools.RectangleContains((int)nX, (int)nY, (int)nWidth, (int)nHeight, pos.X, pos.Y) && doesPatternContainsColor(pattern, pos.Point)) {
                                if (twoDPatterns[twoDPatternsIndex + 1] == null) {
                                    break;
                                } else {
                                    continue;
                                }
                            } else {
                                while (twoDPatterns[twoDPatternsIndex] != null) { twoDPatternsIndex++; };

                                if (twoDPatternsIndex == twoDPatterns.Count - 1) {
                                    goto @continue;
                                }
                            }
                        }
                    }

                    if (crop) { // must be at the end!!
                        point.X -= (int)nX;
                        point.Y -= (int)nY;
                    }

                    if (maxPixelAmount != null) {
                        pixelCounter++;
                    }

                    yield return new Pixel(new RGBColor(r, g, b), new Position(IntPtr.Zero, EPointType.Client, point));

                    @continue:
                    ;
                }
            }

            if (debug) {
                System.IO.File.WriteAllLines($"debug/debug file at {DateTime.Now.Ticks}.txt", debugDict.Select(x => x.Value));
            }

            exit:
            ;
        }

        private byte* getClrBytes(int x, int y) => BitmapData.ScreenData + y * BitmapData.Stride + x * 4; //32bpp, 4 bytes per pixel

        private void getClrBytes(int x, int y, out byte r, out byte g, out byte b, out byte a)
        {
            var s = getClrBytes(x, y);
            b = s[0];
            g = s[1];
            r = s[2];
            a = s[3];
        }

        private bool doesPatternContainsColor(ITwoDimensionalPattern pattern, Point patternPoint)
        {
            if (pattern.ColorSupport) {
                getClrBytes(patternPoint.X, patternPoint.Y, out byte subR, out byte subG, out byte subB, out byte _);
                pattern.GetColor(out var color);

                if (!color.Compare(subR, subG, subB)) {
                    return false;
                }
            }
            //
            return true;
        }

        /// <summary>
        /// Only for one liner like new TRefSnapshot().Scan().IteratePixelsOut(...)
        /// It DOES dispose the snapshot.
        /// </summary>
        /// <param name="buildBitmapOut">(callback, snapshot) => {}</param>
        /// <returns></returns>
        public List<Pixel> IteratePixelsOut(Func<DelegateIteratePixelsAny, BitmapShot, IEnumerable<Pixel>> iteratePixelsOut)
        {
            var list = iteratePixelsOut(IteratePixelsAny, this).ToList();
            //Dispose();
            return list;
        }

        /// <summary>
        /// Only for one liner like new TRefSnapshot().Scan().IteratePixelsOut(...)
        /// It DOES dispose the snapshot.
        /// </summary>
        /// <param name="buildBitmapOut">(callback, snapshot) => {}</param>
        /// <returns></returns>
        public List<Pixel> IteratePixelsOut(Func<DelegateIteratePixelsAny, IEnumerable<Pixel>> iteratePixelsOut)
        {
            var list = iteratePixelsOut(IteratePixelsAny).ToList();
            //Dispose();
            return list;
        }

        public delegate Bitmap DelegateBuildBitmapAny([Optional] int? nX, [Optional] int? nY, [Optional] int? nWidth, [Optional] int? nHeight, [Optional] int? nYHeight, [Optional] int? nXWidth, [Optional] Rectangle? nRect, [Optional] bool nCircle, [Optional] IList<Point> nPolygon, [Optional] List<ITwoDimensionalPattern> twoDPatterns, [Optional] int? maxPixelAmount, [Optional] bool crop, [Optional] IEnumerable<RGBColor> colors, [Optional] bool negateColorResult, [Optional] IEnumerable<ColorTranslation> translateColorList, [Optional] IEnumerable<Area> excludedAreas, [Optional] bool precalculated, [Optional] bool debug, [Optional] ECancelScanIteration cancelScanIteration);

        public Bitmap BuildBitmapAny([Optional] int? nX, [Optional] int? nY, [Optional] int? nWidth, [Optional] int? nHeight, [Optional] int? nYHeight, [Optional] int? nXWidth, [Optional] Rectangle? nRect, [Optional] bool nEllipse, [Optional] IList<Point> nPolygon, [Optional] List<ITwoDimensionalPattern> twoDPatterns, [Optional] int? maxPixelAmount, [Optional] bool crop, [Optional] IEnumerable<RGBColor> colors, [Optional] bool negateColorResult, [Optional] IEnumerable<ColorTranslation> translateColorList, [Optional] IEnumerable<Area> excludedAreas, [Optional] bool precalculated, [Optional] bool debug, [Optional] ECancelScanIteration cancelScanIteration)
        {
            Area.VerifyValues(this, ref nX, ref nY, ref nWidth, ref nHeight, nRect, nEllipse, nPolygon, cancelScanIteration, ref precalculated, ref nYHeight, ref nXWidth);

            var bitmap = new Bitmap(crop ? (int)nWidth : BitmapData.Rectangle.Width, crop ? (int)nHeight : BitmapData.Rectangle.Height, PixelFormat.Format24bppRgb);
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            var bmpDataPointer = (byte*)bmpData.Scan0.ToPointer();

            foreach (Pixel clrPos in IteratePixelsAny(nX, nY, nWidth, nHeight, nYHeight, nXWidth, nRect, nEllipse, nPolygon, twoDPatterns, maxPixelAmount, crop, colors, negateColorResult, translateColorList, excludedAreas, true, debug, cancelScanIteration)) {
                var index = bmpData.Stride * clrPos.Position.Y + 3 * clrPos.Position.X;
                bmpDataPointer[index] = clrPos.Color.B;
                bmpDataPointer[index + 1] = clrPos.Color.G;
                bmpDataPointer[index + 2] = clrPos.Color.R;
            }

            bitmap.UnlockBits(bmpData);

            return bitmap;
        }

        /// <summary>
        /// Only for one liner like new TRefSnapshot().Scan().BuildBitmapOut(...)
        /// It DOES dispose the snapshot.
        /// </summary>
        /// <param name="buildBitmapOut">(callback, snapshot) => {}</param>
        /// <returns></returns>
        public Bitmap BuildBitmapOut(Func<DelegateBuildBitmapAny, BitmapShot, Bitmap> buildBitmapOut)
        {
            var bitmap = buildBitmapOut(BuildBitmapAny, this);
            //Dispose();
            return bitmap;
        }

        /// <summary>
        /// Only for one liner like new TRefSnapshot().Scan().BuildBitmapOut(...)
        /// It DOES dispose the snapshot.
        /// </summary>
        /// <param name="buildBitmapOut">(callback, snapshot) => {}</param>
        /// <returns></returns>
        public Bitmap BuildBitmapOut(Func<DelegateBuildBitmapAny, Bitmap> buildBitmapOut)
        {
            var bitmap = buildBitmapOut(BuildBitmapAny);
            //Dispose();
            return bitmap;
        }

        public void SetPixel(int x, int y, Color clr)
        {
            var bytesPtr = getClrBytes(x, y);
            bytesPtr[0] = clr.B;
            bytesPtr[1] = clr.G;
            bytesPtr[2] = clr.R;
            bytesPtr[3] = clr.A;
        }
    }
}
