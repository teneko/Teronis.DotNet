using System;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectInterceptorDescriptor : IEquatable<JSFunctionalObjectInterceptorDescriptor>
    {
        public IJSFunctionalObjectInterceptor? Implementation { get; }
        public bool HasImplementation => !(Implementation is null);
        public Type ImplementationType { get; }

        internal JSFunctionalObjectInterceptorDescriptor(IJSFunctionalObjectInterceptor implementation, Type? implementationType)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            ImplementationType = implementationType ?? implementation.GetType();
        }

        internal JSFunctionalObjectInterceptorDescriptor(Type implementationType) =>
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));

        public bool Equals(JSFunctionalObjectInterceptorDescriptor? other) =>
            !(other is null) && ImplementationType == other.ImplementationType;
    }
}
