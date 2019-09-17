using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class ResourceManagerExtensions
    {
        public static List<CultureInfo> GetSupportedCultures(this ResourceManager resourceManager)
            => ResourceManagerTools.GetSupportedCultures(resourceManager);
    }
}
