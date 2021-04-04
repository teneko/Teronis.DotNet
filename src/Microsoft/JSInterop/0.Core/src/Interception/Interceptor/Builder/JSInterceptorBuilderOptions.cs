// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    public class JSInterceptorBuilderOptions<DerivedType>
        where DerivedType : JSInterceptorBuilderOptions<DerivedType>
    {
        public IValueAssignerOptions ValueAssignerOptions {
            get {
                if (propertyAssignerOptions is null) {
                    throw new InvalidOperationException("The property assigner options has not been set up.");
                }

                return propertyAssignerOptions;
            }
        }

        /// <summary>
        /// A list of <see cref="IValueAssigner"/> that is lazy initialized.
        /// </summary>
        internal IValueAssignerList ValueAssigners {
            get {
                if (propertyAssigners is null) {
                    throw new InvalidOperationException("The property assigners has not been set up.");
                }

                return propertyAssigners;
            }
        }

        internal virtual JSInterceptorBuilder InterceptorBuilder {
            get {
                if (interceptorBuilder is null) {
                    interceptorBuilder = new JSInterceptorBuilder();
                    isInterceptorBuilderUserTouched = true;
                }

                return interceptorBuilder;
            }
        }

        private JSInterceptorBuilder? interceptorBuilder;
        private IValueAssignerOptions? propertyAssignerOptions;
        private IValueAssignerList? propertyAssigners;
        private bool isInterceptorBuilderUserTouched;

        internal void SetValueAssignerOptions(IValueAssignerOptions propertyAssignerOptions) =>
            this.propertyAssignerOptions = propertyAssignerOptions ?? throw new ArgumentNullException(nameof(propertyAssignerOptions));

        internal void SetValueAssignerList(IValueAssignerList propertyAssigners) =>
            this.propertyAssigners = propertyAssigners ?? throw new ArgumentNullException(nameof(propertyAssigners));

        internal bool TryCreateInterceptorBuilderUserUntouched()
        {
            if (isInterceptorBuilderUserTouched) {
                return false;
            }

            if (interceptorBuilder is null) {
                interceptorBuilder = new JSInterceptorBuilder();
            }

            return true;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSInterceptorServiceCollection"/>.
        /// </summary>
        /// <param name="configure"></param>
        public DerivedType ConfigureInterceptorBuilder(Action<IJSInterceptorServiceCollection> configure)
        {
            configure?.Invoke(InterceptorBuilder);
            return (DerivedType)this;
        }
    }
}
