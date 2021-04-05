// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    public class JSDynamicModuleAssigner : IValueAssigner
    {
        private readonly IJSDynamicModuleActivator jsDynamicModuleActivator;

        public JSDynamicModuleAssigner(IJSDynamicModuleActivator jsDynamicModuleActivator) =>
            this.jsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask AssignValueAsync(IMemberDefinition definition, ValueAssignerContext context)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<AssignDynamicModuleAttribute, JSModuleAttribute>(
                definition,
                out var propertyAttribute,
                out var moduleNameOrPath)) {
                return;
            }

            var interfaceToBeProxied = propertyAttribute.InterfaceToBeProxied ?? definition.MemberType;
            var jsFacade = await jsDynamicModuleActivator.CreateInstanceAsync(interfaceToBeProxied, moduleNameOrPath);
            context.SetValueResult(jsFacade);
        }
    }
}
