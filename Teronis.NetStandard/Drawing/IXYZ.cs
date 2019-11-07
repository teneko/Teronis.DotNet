using System;

namespace Teronis.Drawing
{
    public interface IXYZ : IXY, IEquatable<IXYZ>
    {
        int Z { get; }
    }
}
