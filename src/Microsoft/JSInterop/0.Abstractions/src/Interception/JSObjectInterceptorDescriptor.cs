// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSObjectInterceptorDescriptor : IEquatable<JSObjectInterceptorDescriptor>
    {
        public InterceptorDescriptorRegistrationPhase RegistrationPhase { get; }
        public IJSObjectInterceptor? Implementation { get; }
        public bool HasImplementation => !(Implementation is null);
        public Type ImplementationType { get; }

        internal JSObjectInterceptorDescriptor(
            InterceptorDescriptorRegistrationPhase registrationPhase,
            IJSObjectInterceptor implementation,
            Type? implementationType)
        {
            RegistrationPhase = registrationPhase;
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            ImplementationType = implementationType ?? implementation.GetType();
        }

        internal JSObjectInterceptorDescriptor(InterceptorDescriptorRegistrationPhase registrationPhase, Type implementationType)
        {
            RegistrationPhase = registrationPhase;
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public bool Equals(JSObjectInterceptorDescriptor? other)
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
