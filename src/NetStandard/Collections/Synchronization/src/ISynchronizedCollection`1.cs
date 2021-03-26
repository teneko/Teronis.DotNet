// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizedCollection<ItemType> :
        IEnumerable<ItemType>, IEnumerable, IReadOnlyCollection<ItemType>, IReadOnlyList<ItemType>,
        INotifyCollectionSynchronizing<ItemType>, INotifyCollectionModification<ItemType>, INotifyCollectionChanged, INotifyCollectionSynchronized<ItemType>
    {
        new ItemType this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ItemType> GetEnumerator();
    }
}
