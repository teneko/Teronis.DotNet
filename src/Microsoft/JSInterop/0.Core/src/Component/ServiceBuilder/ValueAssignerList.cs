// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.Assigners;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public class ValueAssignerList<TDerivedValueAssignerOptions> : IValueAssignerList
        where TDerivedValueAssignerOptions : ValueAssignerOptions<TDerivedValueAssignerOptions>
    {
        public int Count =>
            ValueAssignerItems.Count;

        internal TDerivedValueAssignerOptions Options { get; }

        protected virtual List<IValueAssigner> ValueAssignerItems {
            get {
                EnsureCreateValueAssigners();
                return valueAssigners;
            }
        }

        private List<IValueAssigner> valueAssigners;
        private IServiceProvider serviceProvider;
        private bool areValueAssignersCreated;

        public ValueAssignerList(IOptions<TDerivedValueAssignerOptions> options, IServiceProvider serviceProvider)
        {
            valueAssigners = new List<IValueAssigner>();
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IValueAssigner this[int index] =>
            ValueAssignerItems[index];

        internal void SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        private void EnsureCreateValueAssigners()
        {
            if (areValueAssignersCreated) {
                return;
            }

            foreach (var descriptor in Options.Services) {
                var assignerType = descriptor.ImplementationType;
                var assignerFactory = descriptor.ImplementationFactory;

                IValueAssigner assigner;

                if (assignerFactory is null) {
                    assigner = (IValueAssigner)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, assignerType!);
                } else {
                    assigner = (IValueAssigner)assignerFactory.Invoke(serviceProvider);
                }

                valueAssigners.Add(assigner);
            }

            areValueAssignersCreated = true;
        }

        public IEnumerator<IValueAssigner> GetEnumerator() =>
            ValueAssignerItems.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ValueAssignerItems.GetEnumerator();
    }
}
