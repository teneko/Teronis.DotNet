// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners.Builder
{
    public class ValueAssignerOptions<TDerived> : IValueAssignerOptions
        where TDerived : ValueAssignerOptions<TDerived>
    {
        /// <summary>
        /// The property assigner factories.
        /// </summary>
        public ValueAssignerServiceCollection Factories {
            get {
                if (factories is null) {
                    factories = new ValueAssignerServiceCollection();
                    areValueAssignerFactoriesUserTouched = true;
                }

                return factories;
            }
        }

        private ValueAssignerServiceCollection? factories;
        private bool areValueAssignerFactoriesUserTouched;

        public ValueAssignerOptions()
        {
            areValueAssignerFactoriesUserTouched = false;
        }

        internal bool TryCreateValueAssignerFactoriesUserUntouched()
        {
            if (areValueAssignerFactoriesUserTouched) {
                return false;
            }

            if (factories is null) {
                factories = new ValueAssignerServiceCollection();
            }

            return true;
        }


    }
}
