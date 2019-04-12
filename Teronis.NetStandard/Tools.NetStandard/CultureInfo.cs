using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Teronis.Tools.NetStandard
{
    public static class CultureInfoTools
    {
        public static bool DoesCultureNameExists(string name)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var culture in cultures)
                if (culture.Name == name)
                    return true;

            return false;
        }
    }
}
