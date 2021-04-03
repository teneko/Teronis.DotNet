// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace Teronis.DependencyInjection
{
    public interface ILifetimeServiceCollection<TDescriptor> : IReadOnlyLifetimeServiceCollection<TDescriptor>, IList<TDescriptor>, ICollection<TDescriptor>, IEnumerable<TDescriptor>, IEnumerable
    {
        new int Count { get; }

        new TDescriptor this[int index] { get; set; }
    }
}
