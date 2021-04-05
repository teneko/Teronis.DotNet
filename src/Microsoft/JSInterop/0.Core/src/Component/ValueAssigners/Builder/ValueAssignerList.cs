// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners.Builder
{
    public class ValueAssignerList<TDerivedValueAssignerOptions> : IValueAssignerList
        where TDerivedValueAssignerOptions : ValueAssignerOptions<TDerivedValueAssignerOptions>
    {
        internal TDerivedValueAssignerOptions Options { get; }

        public int Count =>
            Items.Count;

        protected virtual List<IValueAssigner> Items {
            get {
                EnsureCreateValueAssignersFromFactories();
                return propertyAssigners;
            }
        }

        private List<IValueAssigner> propertyAssigners;
        private IServiceProvider serviceProvider;
        private bool areValueAssignersCreated;

        public ValueAssignerList(IOptions<TDerivedValueAssignerOptions> options, IServiceProvider serviceProvider)
        {
            propertyAssigners = new List<IValueAssigner>();
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IValueAssigner this[int index] =>
            Items[index];

        internal void SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private void EnsureCreateValueAssignersFromFactories()
        {
            if (areValueAssignersCreated) {
                return;
            }

            foreach (var typeAndFactory in Options.Factories) {
                var componentMemberAssignmentType = typeAndFactory.ImplementationType;
                var componentMemberAssignmentFactory = typeAndFactory.ImplementationFactory;

                IValueAssigner componentMemberAssignment;

                if (componentMemberAssignmentFactory is null) {
                    componentMemberAssignment = (IValueAssigner)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, componentMemberAssignmentType!);
                } else {
                    componentMemberAssignment = (IValueAssigner)componentMemberAssignmentFactory.Invoke(serviceProvider);
                }

                propertyAssigners.Add(componentMemberAssignment);
            }

            areValueAssignersCreated = true;
        }

        public IEnumerator<IValueAssigner> GetEnumerator() =>
            Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Items.GetEnumerator();
    }
}
