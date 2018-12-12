using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public interface IPbkdf2Hash
    {
        string Hash { get; set; }
        string Salt { get; set; }
        int Interations { get; set; }
    }
}
