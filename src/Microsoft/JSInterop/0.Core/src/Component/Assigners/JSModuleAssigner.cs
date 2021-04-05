// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    /// <summary>
    /// Assigns module.
    /// </summary>
    public class JSModuleAssigner : IValueAssigner
    {
        private readonly IJSModuleActivator jsModuleActivator;

        public JSModuleAssigner(IJSModuleActivator jsModuleActivator) =>
            this.jsModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask AssignValueAsync(IMemberDefinition componentMember, ValueAssignerContext context)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<AssignModuleAttribute, JSModuleClassAttribute>(componentMember, out var _, out var moduleNameOrPath)) {
                return;
            }

            var jsModule = await jsModuleActivator.CreateInstanceAsync(moduleNameOrPath);
            context.SetValueResult(jsModule);
        }
    }
}
