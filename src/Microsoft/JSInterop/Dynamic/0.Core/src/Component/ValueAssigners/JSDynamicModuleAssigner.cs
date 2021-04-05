// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners
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
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<ReturnDynamicModuleAttribute, JSDynamicModuleClassAttribute>(
                definition,
                out var moduleNameOrPath)) {
                return;
            }

            var jsFacade = await jsDynamicModuleActivator.CreateInstanceAsync(definition.MemberType, moduleNameOrPath);
            context.SetValueResult(jsFacade);
        }
    }
}
