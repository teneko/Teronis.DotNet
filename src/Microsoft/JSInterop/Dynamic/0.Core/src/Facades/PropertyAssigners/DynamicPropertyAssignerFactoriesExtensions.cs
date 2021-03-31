// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Facades.PropertyAssigners
{
    public static class DynamicPropertyAssignerFactoriesExtensions
    {
        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static PropertyAssignerFactories AddDefaultDynamicPropertyAssigners(this PropertyAssignerFactories propertyAssignerFactories)
        {
            propertyAssignerFactories.Add(typeof(JSDynamicModulePropertyAssigner), value: null);
            return propertyAssignerFactories;
        }
    }
}
