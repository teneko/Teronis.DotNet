// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public abstract class SynchronizingCollectionOptionsBase<TDerived, TItem>
        where TDerived : SynchronizingCollectionOptionsBase<TDerived, TItem>
        where TItem : notnull
    {
        public ICollectionSynchronizationMethod<TItem, TItem>? SynchronizationMethod { get; set; }

        public TDerived SetSynchronizationMethod(ICollectionSynchronizationMethod<TItem, TItem> synchronizationMethod)
        {
            SynchronizationMethod = synchronizationMethod;
            return (TDerived)this;
        }

        public TDerived SetSequentialSynchronizationMethod(IEqualityComparer<TItem> equalityComparer)
        {
            SynchronizationMethod = CollectionSynchronizationMethod.Sequential(equalityComparer);
            return (TDerived)this;
        }

        public TDerived SetSortedSynchronizationMethod(IComparer<TItem> equalityComparer)
        {
            SynchronizationMethod = CollectionSynchronizationMethod.Sorted(equalityComparer);
            return (TDerived)this;
        }
    }
}
