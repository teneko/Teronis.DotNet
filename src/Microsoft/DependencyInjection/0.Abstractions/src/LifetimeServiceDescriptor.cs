// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public abstract class LifetimeServiceDescriptor
    {
        public Type ServiceType { get; }
        public Type? ImplementationType { get; }
        public object? ImplementationInstance { get; }
        public Func<IServiceProvider, object>? ImplementationFactory { get; }

        internal protected LifetimeServiceDescriptor(ServiceDescriptor serviceDescriptor)
        {
            ServiceType = serviceDescriptor.ServiceType;
            ImplementationType = serviceDescriptor.ImplementationType;
            ImplementationInstance = serviceDescriptor.ImplementationInstance;
            ImplementationFactory = serviceDescriptor.ImplementationFactory;
        }

        internal LifetimeServiceDescriptor(Type singletonType) =>
            ServiceType = singletonType ?? throw new ArgumentNullException(nameof(singletonType));

        public LifetimeServiceDescriptor(Type singletonType, Type implementationType)
            : this(singletonType) =>
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));

        /// <summary>
        /// Initializes a new instance of <see cref="LifetimeServiceDescriptor"/> with the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="singletonType">The type of the singleton.</param>
        /// <param name="instance">The instance implementing the singleton.</param>
        public LifetimeServiceDescriptor(Type singletonType, object instance)
            : this(singletonType) =>
            ImplementationInstance = instance ?? throw new ArgumentNullException(nameof(instance));

        /// <summary>
        /// Initializes a new instance of <see cref="LifetimeServiceDescriptor"/> with the specified <paramref name="factory"/>.
        /// </summary>
        /// <param name="singletonType">The type of the singleton.</param>
        /// <param name="factory">A factory used for creating singleton instances.</param>
        public LifetimeServiceDescriptor(Type singletonType, Func<IServiceProvider, object> factory)
            : this(singletonType) =>
            ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));

        internal Type? GetImplementationType()
        {
            if (ImplementationType != null) {
                return ImplementationType;
            }

            if (ImplementationInstance != null) {
                return ImplementationInstance.GetType();
            }

            return ImplementationFactory?.GetType().GenericTypeArguments[1];
        }
    }
}
