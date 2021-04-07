// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;

namespace Teronis.Microsoft.JSInterop.Module
{
    /// <summary>
    /// Activates a dynamic module from <typeparamref name="TModule"/>.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    /// <typeparam name="TModuleActivator"></typeparam>
    internal abstract class JSDynamicModuleTypeActivatorBase<TModule, TModuleActivator>
        where TModule : class, IJSModule
        where TModuleActivator : class
    {
        private readonly IServiceProvider serviceProvider;

        public JSDynamicModuleTypeActivatorBase(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private TModuleActivator GetModuleActivatorOrThrow() =>
            (TModuleActivator)(serviceProvider.GetService(typeof(TModuleActivator))
                ?? throw new InvalidOperationException("Module activator is not registered"));

        protected abstract ValueTask<TModule> ActivateModuleAsync(TModuleActivator moduleActivator, string moduleNameOrPath);

        public ValueTask<TModule> CreateModuleAsync() {
            var moduleActivator = GetModuleActivatorOrThrow();
            var moduleType = typeof(TModule);
            var attributeType = typeof(JSModuleAttribute);

            if (!moduleType.IsDefined(attributeType, inherit: false)) {
                throw new ArgumentException($"The type of {typeof(TModule)} has to be decorated with {typeof(JSModuleAttribute)} to be able to provide the module name or path.");
            }

            var attribute = (JSModuleAttribute)moduleType.GetCustomAttributes(attributeType, inherit: false).Single();
            return ActivateModuleAsync(moduleActivator, attribute.NameOrPath);
        }
    }
}
