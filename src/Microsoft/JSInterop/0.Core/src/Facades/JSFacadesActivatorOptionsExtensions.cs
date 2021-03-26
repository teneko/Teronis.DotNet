// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class JSFacadesActivatorOptionsExtensions
    {
        public static void AddDefaultComponentPropertyAssigners(this JSFacadesActivatorOptions options)
        {
            ComponentPropertyAssignersUtils.ForEachDefaultComponentPropertyAssigner(defaultComponentPropertyAssignerType =>
                options.ComponentPropertyAssignerFactories.Add(defaultComponentPropertyAssignerType, value: null));
        }
    }
}
