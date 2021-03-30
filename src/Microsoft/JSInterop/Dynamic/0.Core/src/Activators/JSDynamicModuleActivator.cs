// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Internals.Utils;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Dynamic.Activators
{
    public class JSDynamicModuleActivator : IInstanceActivatorBase<IJSModule>, IJSDynamicModuleActivator
    {
        private readonly IJSModuleActivator jsModuleActivator;
        private readonly IJSDynamicProxyActivator jSDynamicProxyActivator;

        public JSDynamicModuleActivator(IJSModuleActivator jsModuleActivator, IJSDynamicProxyActivator jSDynamicProxyActivator)
        {
            this.jsModuleActivator = jsModuleActivator;
            this.jSDynamicProxyActivator = jSDynamicProxyActivator;
        }

        public virtual async ValueTask<IJSModule> CreateInstanceAsync(Type interfaceToBeProxied, string moduleNameOrPath)
        {
            TypeUtils.EnsureInterfaceTypeIsAssignaleTo<IJSModule>(interfaceToBeProxied);
            var jsModule = await jsModuleActivator.CreateInstanceAsync(moduleNameOrPath);
            var jsDynamicModule = (IJSModule)jSDynamicProxyActivator.CreateInstance(interfaceToBeProxied, jsModule);
            DispatchInstanceActicated(jsDynamicModule);
            return jsDynamicModule;
        }
    }
}
