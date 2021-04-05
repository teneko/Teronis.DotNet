// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component.Assigners;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public static class DynamicValueAssignerServicesExtensions
    {
        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddDefaultDynamicValueAssigners(this ValueAssignerServiceCollection propertyAssignerFactories)
        {
            propertyAssignerFactories.UseExtension(extension => {
                extension.AddScoped<JSDynamicModuleAssigner>();
                extension.AddScoped<JSDynamicGlobalObjectAssigner>();
            });

            return propertyAssignerFactories;
        }
    }
}
