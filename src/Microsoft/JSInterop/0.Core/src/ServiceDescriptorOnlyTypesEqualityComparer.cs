// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop
{
    internal class ServiceDescriptorOnlyTypesEqualityComparer : EqualityComparer<ServiceDescriptor>
    {
        public new static ServiceDescriptorOnlyTypesEqualityComparer Default = new ServiceDescriptorOnlyTypesEqualityComparer();

        public override bool Equals(ServiceDescriptor? x, ServiceDescriptor? y)
        {
            if (x is null && y is null) {
                return true;
            }

            if (x is null || y is null) {
                return false;
            }

            return x.ServiceType == y.ServiceType
                && x.ImplementationType == y.ImplementationType;
        }

        public override int GetHashCode([DisallowNull] ServiceDescriptor obj) =>
            HashCode.Combine(
                obj.ImplementationFactory,
                obj.ImplementationInstance,
                obj.ImplementationType,
                obj.Lifetime,
                obj.ServiceType);
    }
}
