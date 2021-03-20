using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Collections.Specialized;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadesActivatorOptions
    {
        /// <summary>
        /// Contains factories for component property assignments.
        /// </summary>
        public OrderedDictionary<Type, Func<IServiceProvider, IComponentPropertyAssigner>?> ComponentPropertyAssignerFactories {
            get {
                if (componentPropertyAssignerFactories is null) {
                    componentPropertyAssignerFactories = new OrderedDictionary<Type, Func<IServiceProvider, IComponentPropertyAssigner>?>();
                }

                return componentPropertyAssignerFactories;
            }
        }

        internal List<IComponentPropertyAssigner> ComponentPropertyAssigners {
            get {
                CheckForAddableCollectibleComponentPropertyAssigners();
                EnsureCreatedComponentPropertyAssignersFromFactories();
                return componentPropertyAssigners;
            }
        }

        private OrderedDictionary<Type, Func<IServiceProvider, IComponentPropertyAssigner>?>? componentPropertyAssignerFactories;
        private List<IComponentPropertyAssigner> componentPropertyAssigners;
        private IServiceProvider? serviceProvider;
        private bool areComponentPropertyAssignersCreatedFromFactories;

        public JSFacadesActivatorOptions() =>
            componentPropertyAssigners = new List<IComponentPropertyAssigner>();

        internal void SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider;

        private IServiceProvider GetServiceProviderOrThrow()
        {
            if (serviceProvider is null) {
                throw new InvalidOperationException(nameof(serviceProvider));
            }

            return serviceProvider;
        }

        private void CheckForAddableCollectibleComponentPropertyAssigners()
        {
            if (!(componentPropertyAssignerFactories is null)) {
                return;
            }

            var serviceProvider = GetServiceProviderOrThrow();
            var collectibleComponentPropertyAssignments = serviceProvider.GetServices<ICollectibleComponentPropertyAssigner>();
            var componentPropertyAssignmentFactoryDictionary = ComponentPropertyAssignerFactories;

            foreach (var collectibleComponentPropertyAssignment in collectibleComponentPropertyAssignments) {
                componentPropertyAssignmentFactoryDictionary.Add(collectibleComponentPropertyAssignment.ImplementationType, value: null);
            }
        }

        private void EnsureCreatedComponentPropertyAssignersFromFactories()
        {
            if (areComponentPropertyAssignersCreatedFromFactories) {
                return;
            }

            var serviceProvider = GetServiceProviderOrThrow();

            foreach (var typeAndFactory in ComponentPropertyAssignerFactories) {
                var (componentPropertyAssignmentType, componentPropertyAssignmentFactory) = typeAndFactory;
                IComponentPropertyAssigner componentPropertyAssignment;

                if (componentPropertyAssignmentFactory is null) {
                    componentPropertyAssignment = (IComponentPropertyAssigner)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, componentPropertyAssignmentType);
                } else {
                    componentPropertyAssignment = componentPropertyAssignmentFactory.Invoke(serviceProvider);
                }

                componentPropertyAssigners.Add(componentPropertyAssignment);
            }

            areComponentPropertyAssignersCreatedFromFactories = true;
        }

        internal JSFacades<TJSFacadeActivators> CreateJSFacades<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators
        {
            var serviceProvider = GetServiceProviderOrThrow();
            var jsFacadeActivators = ActivatorUtilities.GetServiceOrCreateInstance<TJSFacadeActivators>(serviceProvider);
            return ActivatorUtilities.CreateInstance<JSFacades<TJSFacadeActivators>>(serviceProvider, jsFacadeActivators);
        }
    }
}
