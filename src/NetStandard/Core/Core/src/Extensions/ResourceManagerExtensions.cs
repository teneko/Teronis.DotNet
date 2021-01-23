using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class ResourceManagerExtensions
    {
        public static List<CultureInfo> GetSupportedCultures(this ResourceManager resourceManager)
            => ResourceManagerUtils.GetSupportedCultures(resourceManager);
    }
}
