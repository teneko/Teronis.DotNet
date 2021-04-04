// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder
{
    public static class ValueAssignerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddNonDynamicDefaultValueAssigners(this ValueAssignerServiceCollection propertyAssignerFactories)
        {
            propertyAssignerFactories.UseExtension(extension => extension.AddScoped<JSModuleMemberAssigner>());
            return propertyAssignerFactories;
        }

        /// <summary>
        /// Adds <see cref="JSCustomFacadeAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection AddJSCustomFacadeAssigner(this ValueAssignerServiceCollection propertyAssignerFactories)
        {
            propertyAssignerFactories.UseExtension(extension => extension.AddScoped<JSCustomFacadeAssigner>());
            return propertyAssignerFactories;
        }

        /// <summary>
        /// Remves first occurrence of <see cref="JSCustomFacadeAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerServiceCollection RemoveJSCustomFacadeAssigner(this ValueAssignerServiceCollection propertyAssignerFactories)
        {
            propertyAssignerFactories.UseExtension(extension => extension.RemoveAll<JSCustomFacadeAssigner>());
            return propertyAssignerFactories;
        }
    }
}
