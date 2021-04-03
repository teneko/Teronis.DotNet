// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public class SingletonServiceCollection : LifetimeServiceCollection<SingletonServiceDescriptor>, ISingletonServiceCollection
    {
        public override ServiceLifetime Lifetime => 
            ServiceLifetime.Singleton;
    }
}
