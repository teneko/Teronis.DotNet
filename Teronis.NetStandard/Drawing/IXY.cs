using System;

namespace Teronis.Drawing
{
    public interface IXY : IEquatable<IXY>
    {
        int X { get; }
        int Y { get; }
    }
}
