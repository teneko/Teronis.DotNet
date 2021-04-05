// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public class ValueAssignerOptions<TDerived> : IValueAssignerOptions
        where TDerived : ValueAssignerOptions<TDerived>
    {
        /// <summary>
        /// The property assigner services.
        /// </summary>
        public ValueAssignerServiceCollection Services {
            get {
                if (services is null) {
                    services = new ValueAssignerServiceCollection();
                    areValueAssignerServicesUserTouched = true;
                }

                return services;
            }
        }

        private ValueAssignerServiceCollection? services;
        private bool areValueAssignerServicesUserTouched;

        public ValueAssignerOptions()
        {
            areValueAssignerServicesUserTouched = false;
        }

        internal bool TryCreateValueAssignerFactoriesUserUntouched()
        {
            if (areValueAssignerServicesUserTouched) {
                return false;
            }

            if (services is null) {
                services = new ValueAssignerServiceCollection();
            }

            return true;
        }


    }
}
