using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teronis.Tools.NetStandard
{
    public static class SystemParametersTools
    {
        public static int CalculateKeyboardDelayInterval(int keyboardDelay)
            => (keyboardDelay + 1) * 250;
    }
}
