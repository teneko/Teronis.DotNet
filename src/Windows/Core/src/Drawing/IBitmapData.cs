using System;
using System.Drawing;

namespace Teronis.Windows.Drawing
{
    public interface IBitmapData : IDisposable
    {
        unsafe byte* ScreenData { get; }
        int Stride { get; }
        Rectangle Rectangle { get; }
    }
}
