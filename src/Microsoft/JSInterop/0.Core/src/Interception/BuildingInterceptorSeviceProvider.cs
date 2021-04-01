// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Microsoft.JSInterop.Component;
using Teronis.Microsoft.JSInterop.Internals.Utils;

namespace Teronis.Microsoft.JSInterop.Interception
{
    /// <summary>
    /// This service provider first searches for property assigner that
    /// has the requested service type implemented.
    /// </summary>
    internal class BuildingInterceptorSeviceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly SlimLazy<IEnumerable<IPropertyAssigner>> lazyPropertyAssigners;
        private readonly Dictionary<Type, IPropertyAssigner> propertyAssignerByInterfaceTypeDictionary;

        public BuildingInterceptorSeviceProvider(IServiceProvider serviceProvider, SlimLazy<IEnumerable<IPropertyAssigner>> lazyPropertyAssigners)
        {
            propertyAssignerByInterfaceTypeDictionary = new Dictionary<Type, IPropertyAssigner>();
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.lazyPropertyAssigners = lazyPropertyAssigners ?? throw new ArgumentNullException(nameof(lazyPropertyAssigners));
        }

        private void EnsureCreateAssignerByInterfaceTypeDictionary()
        {
            foreach (var propertyAssigner in lazyPropertyAssigners.Value) {
                if (propertyAssigner is null) {
                    throw new ArgumentException(nameof(lazyPropertyAssigners));
                }

                var propertyAssignerImplementationType = propertyAssigner.GetType();
                var propertyAssignerInterfaceTypes = TypeUtils.GetInterfaces(propertyAssignerImplementationType);
                propertyAssignerByInterfaceTypeDictionary[propertyAssignerImplementationType] = propertyAssigner;

                foreach (var interfaceType in propertyAssignerInterfaceTypes) {
                    propertyAssignerByInterfaceTypeDictionary[interfaceType] = propertyAssigner;
                }
            }
        }

        private bool TryGetPropertyAssigner(Type serviceType, [MaybeNullWhen(false)] out IPropertyAssigner propertyAssigner)
        {
            EnsureCreateAssignerByInterfaceTypeDictionary();

            if (propertyAssignerByInterfaceTypeDictionary.TryGetValue(serviceType, out propertyAssigner)) {
                return true;
            }

            return false;
        }

        public object? GetService(Type serviceType)
        {
            if (TryGetPropertyAssigner(serviceType, out var propertyAssigner)) {
                return propertyAssigner;
            }

            return serviceProvider.GetService(serviceType);
        }
    }
}
