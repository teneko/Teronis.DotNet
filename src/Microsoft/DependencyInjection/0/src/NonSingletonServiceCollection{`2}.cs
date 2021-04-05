// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection
{
    public abstract class NonSingletonServiceCollection<TProvider, TDescriptor> : LifetimeServiceCollection<TProvider, TDescriptor>, INonSingletonServiceCollection<TProvider, TDescriptor>
        where TProvider : class, IServiceProvider
        where TDescriptor : NonSingletonServiceDescriptor<TProvider>
    { }
}
