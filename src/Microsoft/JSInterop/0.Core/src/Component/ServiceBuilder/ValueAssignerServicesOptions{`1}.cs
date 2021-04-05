// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public class ValueAssignerServicesOptions<TDerived> : IValueAssignerServicesOptions
        where TDerived : ValueAssignerServicesOptions<TDerived>
    {
        /// <summary>
        /// The property assigner services.
        /// </summary>
        public ValueAssignerServiceCollection Services {
            get {
                if (services is null) {
                    services = new ValueAssignerServiceCollection();
                    AreValueAssignerServicesUserTouched = true;
                }

                return services;
            }
        }

        internal bool AreValueAssignerServicesUserTouched { get; private set; }

        private ValueAssignerServiceCollection? services;

        public ValueAssignerServicesOptions()
        {
            AreValueAssignerServicesUserTouched = false;
        }

        internal bool TryCreateValueAssignerServicesWhenUserUntouched()
        {
            if (AreValueAssignerServicesUserTouched) {
                return false;
            }

            if (services is null) {
                services = new ValueAssignerServiceCollection();
            }

            return true;
        }


    }
}
