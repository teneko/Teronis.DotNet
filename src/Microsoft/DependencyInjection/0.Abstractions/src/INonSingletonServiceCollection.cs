// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection
{
    public interface INonSingletonServiceCollection<TProvider, TNonSingletonServiceDescriptor> : ILifetimeServiceCollection<TNonSingletonServiceDescriptor>
        where TProvider : class, IServiceProvider
        where TNonSingletonServiceDescriptor : NonSingletonServiceDescriptor<TProvider>
    { }
}
