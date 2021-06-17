// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizedCollection<TItem> :
        INotifyPropertyChanged, INotifyPropertyChanging,
        IEnumerable<TItem>, IEnumerable, IReadOnlyCollection<TItem>, IReadOnlyList<TItem>,
        INotifyCollectionSynchronizing<TItem>, INotifyCollectionModification<TItem>, INotifyCollectionChanged, INotifyCollectionSynchronized<TItem>,
        IObservable<ICollectionModification<TItem, TItem>>
    {
        new TItem this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<TItem> GetEnumerator();
    }
}
