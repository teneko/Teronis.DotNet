// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder
{
    public class ValueAssignerOptions<TDerived> : IValueAssignerOptions
        where TDerived : ValueAssignerOptions<TDerived>
    {
        /// <summary>
        /// The property assigner factories.
        /// </summary>
        public ValueAssignerFactories Factories {
            get {
                if (factories is null) {
                    factories = new ValueAssignerFactories();
                    areValueAssignerFactoriesUserTouched = true;
                }

                return factories;
            }
        }

        private ValueAssignerFactories? factories;
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
                factories = new ValueAssignerFactories();
            }

            return true;
        }


    }
}
