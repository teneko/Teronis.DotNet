// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public interface IReadOnlyLifetimeServiceCollection<out TDescriptor> : IReadOnlyList<TDescriptor>
    {
        ServiceLifetime Lifetime { get; }
    }
}
