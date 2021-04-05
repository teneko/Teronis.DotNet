// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public interface IServiceCollectionAdapter<out TCollection> : IServiceCollection
    {   
        TCollection LifetimeServiceCollection { get; }
    }
}
