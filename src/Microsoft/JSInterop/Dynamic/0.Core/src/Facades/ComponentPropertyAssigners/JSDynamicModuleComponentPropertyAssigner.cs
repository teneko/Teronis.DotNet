// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Facades.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic.Module;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Facades.Annotations;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades.ComponentPropertyAssignments
{
    public class JSDynamicModuleComponentPropertyAssigner : IComponentPropertyAssigner
    {
        private readonly IJSDynamicModuleActivator jsDynamicModuleActivator;

        public JSDynamicModuleComponentPropertyAssigner(IJSDynamicModuleActivator jsDynamicModuleActivator) =>
            this.jsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask<YetNullable<IAsyncDisposable>> TryAssignComponentProperty(IComponentProperty componentProperty)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<JSDynamicModulePropertyAttribute, JSDynamicModuleClassAttribute>(
                componentProperty,
                out var moduleNameOrPath)) {
                return default;
            }

            var jsFacade = (IAsyncDisposable)await jsDynamicModuleActivator.CreateInstanceAsync(componentProperty.PropertyType, moduleNameOrPath);
            return new YetNullable<IAsyncDisposable>(jsFacade);
        }
    }
}
