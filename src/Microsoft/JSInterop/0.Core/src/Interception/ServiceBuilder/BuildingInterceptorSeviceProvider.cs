// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Microsoft.JSInterop.Component.Assigners;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Utils;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    /// <summary>
    /// This service provider first searches for property assigner that
    /// has the requested service type implemented.
    /// </summary>
    internal class BuildingInterceptorSeviceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IValueAssignerList propertyAssigners;
        private readonly Dictionary<Type, IValueAssigner> propertyAssignerByInterfaceTypeDictionary;

        public BuildingInterceptorSeviceProvider(IServiceProvider serviceProvider, IValueAssignerList propertyAssigners)
        {
            propertyAssignerByInterfaceTypeDictionary = new Dictionary<Type, IValueAssigner>();
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.propertyAssigners = propertyAssigners ?? throw new ArgumentNullException(nameof(propertyAssigners));
        }

        private void EnsureCreateAssignerByInterfaceTypeDictionary()
        {
            foreach (var propertyAssigner in propertyAssigners) {
                if (propertyAssigner is null) {
                    throw new ArgumentException(nameof(propertyAssigners));
                }

                var propertyAssignerImplementationType = propertyAssigner.GetType();
                var propertyAssignerInterfaceTypes = TypeUtils.GetInterfaces(propertyAssignerImplementationType);
                propertyAssignerByInterfaceTypeDictionary[propertyAssignerImplementationType] = propertyAssigner;

                foreach (var interfaceType in propertyAssignerInterfaceTypes) {
                    propertyAssignerByInterfaceTypeDictionary[interfaceType] = propertyAssigner;
                }
            }
        }

        private bool TryGetValueAssigner(Type serviceType, [MaybeNullWhen(false)] out IValueAssigner propertyAssigner)
        {
            EnsureCreateAssignerByInterfaceTypeDictionary();

            if (propertyAssignerByInterfaceTypeDictionary.TryGetValue(serviceType, out propertyAssigner)) {
                return true;
            }

            return false;
        }

        public object? GetService(Type serviceType)
        {
            // Allows interceptors to resolve property assigner list.
            if (serviceType == typeof(IValueAssignerList)) {
                return propertyAssigners;
            }

            if (TryGetValueAssigner(serviceType, out var propertyAssigner)) {
                return propertyAssigner;
            }

            return serviceProvider.GetService(serviceType);
        }
    }
}
