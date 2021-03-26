// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
