using System;
using System.Drawing;
using Teronis.Windows;

namespace Teronis.Drawing
{
    public unsafe struct Position : ITwoDimensionalPattern
    {
        /* TODO: reimplement */
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="point">screen point</param>
        ///// <returns></returns>
        //public static Point GetMickeyPoint(PointF point)
        //{
        //    int screenWidth = Win32.GetSystemMetrics(0);
        //    int screenHeight = Win32.GetSystemMetrics(1);

        //    // Mickey X coordinate
        //    int mic_x = (int)Math.Round((point.X) * 65536.0 / screenWidth);
        //    // Mickey Y coordinate
        //    int mic_y = (int)Math.Round((point.Y) * 65536.0 / screenHeight);

        //    return new Point(mic_x, mic_y);
        //}

        public Point Point;
        public EPointType PointType { get; private set; }
        public int X { get { return Point.X; } set { Point.X = value; } }
        public int Y { get { return Point.Y; } set { Point.Y = value; } }

        bool ITwoDimensionalPattern.ColorSupport => false;

        private readonly IntPtr hWnd;

        public Position(IntPtr hWnd, EPointType pointType, Point? point = null)
        {
            this.hWnd = hWnd;
            PointType = pointType;
            Point = point ?? Point.Empty;
        }

        public Position(Point point)
        {
            hWnd = IntPtr.Zero;
            PointType = EPointType.Relative;
            Point = point;
        }

        public Position(int x, int y) : this(new Point(x, y)) { }

        public Point GetClientPoint()
        {
            if (PointType == EPointType.Client) {
                return Point;
            } else {
                return Win32.ScreenToClient(hWnd, Point);
            }
        }

        public Position GetClientPosition()
        {
            return new Position(hWnd, EPointType.Client, GetClientPoint());
        }

        public Point GetScreenPoint()
        {
            if (PointType == EPointType.Screen) {
                return Point;
            } else {
                return Win32.ClientToScreen(hWnd, Point);
            }
        }

        public Position GetScreenPosition()
        {
            return new Position(hWnd, EPointType.Screen, GetScreenPoint());
        }

        /* TODO: reimplement */
        //public Point GetMickeyPoint()
        //{
        //    return GetMickeyPoint(GetScreenPoint());
        //}

        void ITwoDimensionalPattern.GetPosition(out Position position) => position = this;

        void ITwoDimensionalPattern.GetColor(out RGBColor color) => color = RGBColor.Empty;

        public static implicit operator Point(Position position) => position.Point;
    }
}
