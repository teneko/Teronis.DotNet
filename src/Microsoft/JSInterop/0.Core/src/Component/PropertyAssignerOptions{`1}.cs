// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public class PropertyAssignerOptions<TDerived>
        where TDerived : PropertyAssignerOptions<TDerived>
    {
        /// <summary>
        /// The property assigner factories.
        /// </summary>
        public PropertyAssignerFactories Factories {
            get {
                if (factories is null) {
                    factories = new PropertyAssignerFactories();
                    arePropertyAssignersUserTouched = true;
                }

                return factories;
            }
        }

        internal List<IPropertyAssigner> PropertyAssigners {
            get {
                EnsureCreatedPropertyAssignersFromFactories();
                return propertyAssigners;
            }
        }

        private PropertyAssignerFactories? factories;
        private List<IPropertyAssigner> propertyAssigners;
        private IServiceProvider? serviceProvider;
        private bool arePropertyAssignersCreated;
        private bool arePropertyAssignersUserTouched;

        public PropertyAssignerOptions()
        {
            propertyAssigners = new List<IPropertyAssigner>();
            arePropertyAssignersUserTouched = false;
        }

        internal bool ArePropertyAssignersUserUntouched()
        {
            if (arePropertyAssignersUserTouched) {
                return false;
            }

            if (propertyAssigners is null) {
                factories = new PropertyAssignerFactories();
            }

            return true;
        }

        internal void SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private IServiceProvider GetServiceProviderOrThrow()
        {
            if (serviceProvider is null) {
                throw new InvalidOperationException(nameof(serviceProvider));
            }

            return serviceProvider;
        }

        private void EnsureCreatedPropertyAssignersFromFactories()
        {
            if (arePropertyAssignersCreated) {
                return;
            }

            var serviceProvider = GetServiceProviderOrThrow();

            foreach (var typeAndFactory in Factories) {
                var (componentPropertyAssignmentType, componentPropertyAssignmentFactory) = typeAndFactory;
                IPropertyAssigner componentPropertyAssignment;

                if (componentPropertyAssignmentFactory is null) {
                    componentPropertyAssignment = (IPropertyAssigner)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, componentPropertyAssignmentType);
                } else {
                    componentPropertyAssignment = componentPropertyAssignmentFactory.Invoke(serviceProvider);
                }

                propertyAssigners.Add(componentPropertyAssignment);
            }

            arePropertyAssignersCreated = true;
        }
    }
}
