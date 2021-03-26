// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Dynamic.Facades.ComponentPropertyAssignments;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class JSFacadesActivatorOptionsExtensions
    {
        /// <summary>
        /// Adds default component property assigners to
        /// <see cref="JSFacadesActivatorOptions.ComponentPropertyAssignerFactories"/>
        /// .
        /// </summary>
        /// <param name="options"></param>
        public static void AddDefaultDynamicComponentPropertyAssigners(this JSFacadesActivatorOptions options)
        {
            ComponentPropertyAssignersUtils.ForEachDefaultComponentPropertyAssigner(defaultComponentPropertyAssignerType =>
                options.ComponentPropertyAssignerFactories.Add(defaultComponentPropertyAssignerType, value: null));
        }
    }
}
