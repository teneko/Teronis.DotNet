// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Teronis.Mvc
{
    public class MvcBuilderContext
    {
        public ApplicationPartManager PartManager { get; }

        public MvcBuilderContext(ApplicationPartManager partManager) {
            PartManager = partManager;
        }
    }
}
