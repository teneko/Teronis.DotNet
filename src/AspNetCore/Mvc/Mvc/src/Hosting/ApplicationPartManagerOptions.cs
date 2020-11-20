using System;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Teronis.Mvc.Hosting
{
    public class ApplicationPartManagerOptions
    {
        public ApplicationPartManager PartManager { get; }

        public ApplicationPartManagerOptions(ApplicationPartManager partManager) {
            PartManager = partManager ?? throw new ArgumentNullException(nameof(partManager));
        }
    }
}
