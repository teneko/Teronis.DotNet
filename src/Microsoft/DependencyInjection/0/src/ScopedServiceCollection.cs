// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public class ScopedServiceCollection : NonSingletonServiceCollection<IServiceProvider, ScopedServiceDescriptor>, IScopedServiceCollection
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;
    }
}
