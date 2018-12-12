using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Teronis.NetStandard.Tools
{
    public static class MathTools
    {
        public static int GetZeroBasedModulo(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static int GetOneBasedModulo(int x, int m)
        {
            int r = x % m;
            return r <= 0 ? r + m : r;
        }
    }
}
