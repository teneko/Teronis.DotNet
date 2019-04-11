using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Drawing
{
    public interface IXYZ : IXY, IEquatable<IXYZ>
    {
        int Z { get; }
    }
}
