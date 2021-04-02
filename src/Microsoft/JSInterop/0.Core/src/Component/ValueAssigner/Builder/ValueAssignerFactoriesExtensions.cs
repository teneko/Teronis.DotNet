// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder
{
    public static class ValueAssignerFactoriesExtensions
    {
        public static void Add<TValueAssigner>(this ValueAssignerFactories factories) =>
            factories.Add(typeof(TValueAssigner));

        public static void Remove<TValueAssigner>(this ValueAssignerFactories factories) =>
            factories.Remove(typeof(TValueAssigner));

        /// <summary>
        /// Adds the default property assigners that resides in this namespace.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerFactories AddNonDynamicDefaultValueAssigners(this ValueAssignerFactories propertyAssignerFactories)
        {
            propertyAssignerFactories.Add<JSModuleMemberAssigner>();
            return propertyAssignerFactories;
        }

        /// <summary>
        /// Adds <see cref="JSCustomFacadeAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerFactories AddJSCustomFacadeAssigner(this ValueAssignerFactories propertyAssignerFactories)
        {
            propertyAssignerFactories.Add<JSCustomFacadeAssigner>();
            return propertyAssignerFactories;
        }

        /// <summary>
        /// Remves first occurrence of <see cref="JSCustomFacadeAssigner"/>.
        /// </summary>
        /// <param name="propertyAssignerFactories"></param>
        /// <returns></returns>
        public static ValueAssignerFactories RemoveJSCustomFacadeAssigner(this ValueAssignerFactories propertyAssignerFactories)
        {
            propertyAssignerFactories.Remove<JSCustomFacadeAssigner>();
            return propertyAssignerFactories;
        }
    }
}
