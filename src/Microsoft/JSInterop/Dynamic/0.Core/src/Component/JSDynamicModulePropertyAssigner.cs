// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public class JSDynamicModulePropertyAssigner : IPropertyAssigner
    {
        private readonly IJSDynamicModuleActivator jsDynamicModuleActivator;

        public JSDynamicModulePropertyAssigner(IJSDynamicModuleActivator jsDynamicModuleActivator) =>
            this.jsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask AssignPropertyAsync(IDefinition definition, PropertyAssignerContext context)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<ReturnDynamicModuleAttribute, JSDynamicModuleClassAttribute>(
                definition,
                out var moduleNameOrPath)) {
                return;
            }

            var jsFacade = (IAsyncDisposable)await jsDynamicModuleActivator.CreateInstanceAsync(definition.MemberType, moduleNameOrPath);
            context.MemberResult = new YetNullable<IAsyncDisposable>(jsFacade);
        }
    }
}
