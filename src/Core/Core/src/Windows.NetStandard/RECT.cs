using System.Drawing;
using System.Runtime.InteropServices;

namespace Teronis.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int GetWidth()
        {
            return right - left;
        }

        public int GetHeight()
        {
            return bottom - top;
        }

        public int SetWidth(int width)
        {
            return right = left + width;
        }

        public int SetHeight(int height)
        {
            return top = top + height;
        }

        public void SetSize(int width, int height)
        {
            SetWidth(width);
            SetHeight(height);
        }

        public void SetSize(Size size)
        {
            SetSize(size.Width, size.Height);
        }

        public int GetX()
        {
            return left;
        }

        public int GetY()
        {
            return top;
        }

        public Point GetLocation()
        {
            return new Point(GetX(), GetY());
        }

        public void GetX(int x)
        {
            var width = GetWidth();
            left = x;
            right = x + width;
        }

        public void GetY(int y)
        {
            var height = GetHeight();
            top = y;
            bottom = y + height;
        }

        public void SetLocation(int x, int y)
        {
            GetX(x);
            GetY(y);
        }

        public Point SetLocation(Point location)
        {
            SetLocation(location.X, location.Y);
            return location;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(GetX(), GetY(), GetWidth(), GetHeight());
        }

        public override string ToString()
        {
            return $"{{{left} {top} {right} {bottom}}}";
        }
    }
}
