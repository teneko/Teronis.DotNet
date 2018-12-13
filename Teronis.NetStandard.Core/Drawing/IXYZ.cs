using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Drawing
{
    public interface IXYZ : IXY, IEquatable<IXYZ>
    {
        int Z { get; }
    }
}
