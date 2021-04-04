// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.DependencyInjection
{
    public abstract class NonSingletonServiceCollection<TNonSingletonServiceDescriptor> : LifetimeServiceCollection<TNonSingletonServiceDescriptor>, INonSingletonServiceCollection<TNonSingletonServiceDescriptor>
        where TNonSingletonServiceDescriptor : NonSingletonServiceDescriptor
    { }
}
