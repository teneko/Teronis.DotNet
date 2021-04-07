// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Module
{
    /// <summary>
    /// Provide the same dynamic module of type <typeparamref name="TModule"/> within this instance.
    /// </summary>
    /// <remarks>
    /// It is intended to be registered as open generic service.
    /// </remarks>
    /// <typeparam name="TModule">The type to be proxied.</typeparam>
    public sealed class JSDynamicModuleProvider<TModule>
        where TModule : class, IJSModule
    {
        public SlimLazy<ValueTask<TModule>> Module =>
            module ??= new SlimLazy<ValueTask<TModule>>(CreateModuleAsync);

        private SlimLazy<ValueTask<TModule>>? module;
        private readonly IServiceProvider serviceProvider;

        public JSDynamicModuleProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private ValueTask<TModule> CreateModuleAsync() =>
            new ModuleProvider(serviceProvider).CreateModuleAsync();

        private class ModuleProvider : JSDynamicModuleTypeActivatorBase<TModule, IJSDynamicModuleActivator> {
            public ModuleProvider(IServiceProvider serviceProvider) 
                : base(serviceProvider) { }

            protected override ValueTask<TModule> ActivateModuleAsync(IJSDynamicModuleActivator moduleActivator, string moduleNameOrPath) =>
                moduleActivator.CreateInstanceAsync<TModule>(moduleNameOrPath);
        }
    }
}
