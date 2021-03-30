// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Facades.PropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeHubActivatorOptions
    {
        /// <summary>
        /// Contains factories for component property assignments.
        /// </summary>
        public PropertyAssignerFactories PropertyAssignerFactories {
            get {
                if (propertyAssignerFactories is null) {
                    propertyAssignerFactories = new PropertyAssignerFactories();
                    arePropertyAssignersDefault = false;
                }

                return propertyAssignerFactories;
            }
        }

        internal List<IPropertyAssigner> PropertyAssigners {
            get {
                EnsureCreatedPropertyAssignersFromFactories();
                return propertyAssigners;
            }
        }

        private PropertyAssignerFactories? propertyAssignerFactories;
        private List<IPropertyAssigner> propertyAssigners;
        private IServiceProvider? serviceProvider;
        private bool arePropertyAssignersCreated;
        private bool arePropertyAssignersDefault;

        public JSFacadeHubActivatorOptions()
        {
            propertyAssigners = new List<IPropertyAssigner>();
            arePropertyAssignersDefault = true;
        }

        internal bool AreDefaultPropertyAssigners() {
            if (!arePropertyAssignersDefault) {
                return false;
            }

            if (propertyAssigners is null) {
                propertyAssignerFactories = new PropertyAssignerFactories();
            }

            return true;
        }

        internal void SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider;

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

            foreach (var typeAndFactory in PropertyAssignerFactories) {
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

        internal JSFacadeHub<TJSFacadeActivators> CreateFacadeHub<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators
        {
            var serviceProvider = GetServiceProviderOrThrow();
            var jsFacadeActivators = ActivatorUtilities.GetServiceOrCreateInstance<TJSFacadeActivators>(serviceProvider);
            return ActivatorUtilities.CreateInstance<JSFacadeHub<TJSFacadeActivators>>(serviceProvider, jsFacadeActivators);
        }
    }
}
