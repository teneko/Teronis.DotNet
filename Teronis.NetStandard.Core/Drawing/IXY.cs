using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Drawing
{
    public interface IXY : IEquatable<IXY>
    {
        int X { get; }
        int Y { get; }
    }
}
