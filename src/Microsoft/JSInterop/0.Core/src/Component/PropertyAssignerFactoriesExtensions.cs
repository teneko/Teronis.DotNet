// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component
{
    public static class PropertyAssignerFactoriesExtensions
    {
        public static void Add<TPropertyAssigner>(this PropertyAssignerFactories factories) =>
            factories.Add(typeof(TPropertyAssigner));

        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static PropertyAssignerFactories AddDefaultPropertyAssigners(this PropertyAssignerFactories propertyAssignerFactories)
        {
            propertyAssignerFactories.Add<JSModuleMemberAssigner>();
            propertyAssignerFactories.Add<JSCustomFacadeAssigner>();
            return propertyAssignerFactories;
        }
    }
}
