// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizedCollection<TItem> :
        IEnumerable<TItem>, IEnumerable, IReadOnlyCollection<TItem>, IReadOnlyList<TItem>,
        INotifyCollectionSynchronizing<TItem>, INotifyCollectionModification<TItem>, INotifyCollectionChanged, INotifyCollectionSynchronized<TItem>
    {
        new TItem this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<TItem> GetEnumerator();
    }
}
