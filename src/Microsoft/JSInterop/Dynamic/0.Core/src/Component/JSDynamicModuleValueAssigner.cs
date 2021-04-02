// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public class JSDynamicModuleValueAssigner : IValueAssigner
    {
        private readonly IJSDynamicModuleActivator jsDynamicModuleActivator;

        public JSDynamicModuleValueAssigner(IJSDynamicModuleActivator jsDynamicModuleActivator) =>
            this.jsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask AssignValueAsync(IDefinition definition, ValueAssignerContext context)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<ReturnDynamicModuleAttribute, JSDynamicModuleClassAttribute>(
                definition,
                out var moduleNameOrPath)) {
                return;
            }

            var jsFacade = (IAsyncDisposable)await jsDynamicModuleActivator.CreateInstanceAsync(definition.MemberType, moduleNameOrPath);
            context.ValueResult = new YetNullable<IAsyncDisposable>(jsFacade);
        }
    }
}
