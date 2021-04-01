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
                    arePropertyAssignerFactoriesUserTouched = true;
                }

                return factories;
            }
        }

        internal List<IPropertyAssigner> PropertyAssigners {
            get {
                EnsureCreatePropertyAssignersFromFactories();
                return propertyAssigners;
            }
        }

        private PropertyAssignerFactories? factories;
        private List<IPropertyAssigner> propertyAssigners;
        private IServiceProvider? serviceProvider;
        private bool arePropertyAssignersCreated;
        private bool arePropertyAssignerFactoriesUserTouched;

        public PropertyAssignerOptions()
        {
            propertyAssigners = new List<IPropertyAssigner>();
            arePropertyAssignerFactoriesUserTouched = false;
        }

        internal bool TryCreatePropertyAssignerFactoriesUserUntouched()
        {
            if (arePropertyAssignerFactoriesUserTouched) {
                return false;
            }

            if (factories is null) {
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

        private void EnsureCreatePropertyAssignersFromFactories()
        {
            if (arePropertyAssignersCreated) {
                return;
            }

            var serviceProvider = GetServiceProviderOrThrow();

            foreach (var typeAndFactory in Factories) {
                var (componentMemberAssignmentType, componentMemberAssignmentFactory) = typeAndFactory;
                IPropertyAssigner componentMemberAssignment;

                if (componentMemberAssignmentFactory is null) {
                    componentMemberAssignment = (IPropertyAssigner)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, componentMemberAssignmentType);
                } else {
                    componentMemberAssignment = componentMemberAssignmentFactory.Invoke(serviceProvider);
                }

                propertyAssigners.Add(componentMemberAssignment);
            }

            arePropertyAssignersCreated = true;
        }
    }
}
