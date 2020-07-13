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
