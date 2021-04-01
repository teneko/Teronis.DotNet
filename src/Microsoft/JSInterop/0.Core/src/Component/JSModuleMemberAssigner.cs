// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public class JSModuleMemberAssigner : IPropertyAssigner
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
        public virtual async ValueTask<YetNullable<IAsyncDisposable>> TryAssignProperty(IDefinition componentProperty)
        {
            if (!JSModuleAttributeUtils.TryGetModuleNameOrPath<AssignModuleAttribute, JSModuleClassAttribute>(componentProperty, out var moduleNameOrPath)) {
                return default;
            }

            var jsModule = await jsModuleActivator.CreateInstanceAsync(moduleNameOrPath);
            IAsyncDisposable disposable;

            if (componentProperty.IsAttributeDefined<AssignCustomFacadeAttribute>()) {
                disposable = jsCustomFacadeActivator.CreateInstance(jsModule, componentProperty.MemberType);
            } else {
                disposable = jsModule;
            }

            return new YetNullable<IAsyncDisposable>(disposable);
        }
    }
}
