using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssignments;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class JSFacadesActivatorOptionsExtensions
    {
        public static JSFacadesActivatorOptions AddDefaultComponentPropertyAssignments(this JSFacadesActivatorOptions options)
        {
            options.ComponentPropertyAssignmentFactories.Add(typeof(JSModuleComponentAssignment), serviceProvider => ActivatorUtilities.CreateInstance<JSModuleComponentAssignment>(serviceProvider));
            return options;
        }
    }
}
