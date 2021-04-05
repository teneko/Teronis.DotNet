// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners.Builder
{
    public static class ValueAssignerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerServices"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddNonDynamicDefaultValueAssigners(this ValueAssignerServiceCollection propertyAssignerServices)
        {
            propertyAssignerServices.UseExtension(extension => {
                extension.AddScoped<JSLocalObjectAssigner>();
                extension.AddScoped<JSModuleAssigner>();
            });

            return propertyAssignerServices;
        }

        /// <summary>
        /// Adds <see cref="JSCustomFacadeAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerServices"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddJSCustomFacadeAssigner(this ValueAssignerServiceCollection propertyAssignerServices)
        {
            propertyAssignerServices.UseExtension(extension => extension.AddScoped<JSCustomFacadeAssigner>());
            return propertyAssignerServices;
        }

        /// <summary>
        /// Remves first occurrence of <see cref="JSCustomFacadeAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerServices"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection RemoveJSCustomFacadeAssigner(this ValueAssignerServiceCollection propertyAssignerServices)
        {
            propertyAssignerServices.UseExtension(extension => extension.RemoveAll<JSCustomFacadeAssigner>());
            return propertyAssignerServices;
        }
    }
}
