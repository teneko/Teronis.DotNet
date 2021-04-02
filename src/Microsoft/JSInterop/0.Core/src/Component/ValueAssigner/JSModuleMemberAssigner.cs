// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner
{
    public class JSModuleMemberAssigner : IValueAssigner
    {
        private readonly IJSModuleActivator jsModuleActivator;
        private readonly IJSCustomFacadeActivator jsCustomFacadeActivator;

        public JSModuleMemberAssigner(
            IJSModuleActivator jsModuleActivator,
            IJSCustomFacadeActivator jsCustomFacadeActivator)
        {
            this.jsModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));
            this.jsCustomFacadeActivator = jsCustomFacadeActivator ?? throw new ArgumentNullException(nameof(jsCustomFacadeActivator));
        }

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual async ValueTask AssignValueAsync(IDefinition componentMember, ValueAssignerContext context)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<AssignModuleAttribute, JSModuleClassAttribute>(componentMember, out var moduleNameOrPath)) {
                return;
            }

            var jsModule = await jsModuleActivator.CreateInstanceAsync(moduleNameOrPath);
            IAsyncDisposable disposable;

            if (componentMember.IsAttributeDefined<AssignCustomFacadeAttribute>()) {
                disposable = jsCustomFacadeActivator.CreateInstance(jsModule, componentMember.MemberType);
            } else {
                disposable = jsModule;
            }

            context.ValueResult = new YetNullable<IAsyncDisposable>(disposable);
        }
    }
}
