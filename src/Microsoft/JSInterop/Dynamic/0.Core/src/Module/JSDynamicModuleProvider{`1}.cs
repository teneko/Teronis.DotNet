// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;

namespace Teronis.Microsoft.JSInterop.Module
{
    /// <summary>
    /// Provides a specific module. It can be registered as open generic type.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public class JSDynamicModuleProvider<TModule>
        where TModule : class, IJSModule
    {
        public SlimLazy<ValueTask<TModule>> Module { get; }

        public JSDynamicModuleProvider(IJSDynamicModuleActivator jsDynamicModuleActivator)
        {
            if (jsDynamicModuleActivator is null) {
                throw new ArgumentNullException(nameof(jsDynamicModuleActivator));
            }

            var moduleType = typeof(TModule);
            var attributeType = typeof(JSModuleAttribute);

            if (!moduleType.IsDefined(attributeType, inherit: false)) {
                throw new ArgumentException($"The type of {typeof(TModule)} has to be decorated with {typeof(JSModuleAttribute)}");
            }

            var attribute = (JSModuleAttribute)moduleType.GetCustomAttributes(attributeType, inherit: false).Single();
            Module = new SlimLazy<ValueTask<TModule>>(() => jsDynamicModuleActivator.CreateInstanceAsync<TModule>(attribute.NameOrPath));
        }
    }
}
