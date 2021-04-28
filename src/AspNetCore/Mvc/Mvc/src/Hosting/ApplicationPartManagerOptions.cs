// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Teronis.AspNetCore.Mvc.Hosting
{
    public class ApplicationPartManagerOptions
    {
        public ApplicationPartManager PartManager { get; }

        public ApplicationPartManagerOptions(ApplicationPartManager partManager) {
            PartManager = partManager ?? throw new ArgumentNullException(nameof(partManager));
        }
    }
}
