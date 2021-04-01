// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptorDescriptor : IEquatable<JSInterceptorDescriptor>
    {
        public InterceptorDescriptorRegistrationPhase RegistrationPhase { get; }
        public IJSInterceptor? Implementation { get; }
        public bool HasImplementation => !(Implementation is null);
        public Type ImplementationType { get; }

        internal JSInterceptorDescriptor(
            InterceptorDescriptorRegistrationPhase registrationPhase,
            IJSInterceptor implementation,
            Type? implementationType)
        {
            RegistrationPhase = registrationPhase;
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            ImplementationType = implementationType ?? implementation.GetType();
        }

        internal JSInterceptorDescriptor(InterceptorDescriptorRegistrationPhase registrationPhase, Type implementationType)
        {
            RegistrationPhase = registrationPhase;
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public bool Equals(JSInterceptorDescriptor? other)
        {
            if (other is null) {
                return false;
            }

            if (HasImplementation && other.HasImplementation) {
                return Implementation == other.Implementation;
            }

            if (HasImplementation || other.HasImplementation) {
                return false;
            }

            return ImplementationType == other.ImplementationType;
        }
    }
}
