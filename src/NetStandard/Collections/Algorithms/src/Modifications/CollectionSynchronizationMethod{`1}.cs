// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public sealed class CollectionSynchronizationMethod<TItem>
        where TItem : notnull
    {
        public class Sequential : CollectionSynchronizationMethod<TItem, TItem, TItem>.Sequential, ICollectionSynchronizationMethod<TItem>
        {
            public Sequential(
                Func<TItem, TItem> getComparablePartOfLeftItem,
                Func<TItem, TItem> getComparablePartOfRightItem,
                IEqualityComparer<TItem> equalityComparer)
                : base(getComparablePartOfLeftItem, getComparablePartOfRightItem, equalityComparer) { }
        }

        public class Sorted : CollectionSynchronizationMethod<TItem, TItem, TItem>.Sorted, ICollectionSynchronizationMethod<TItem>
        {
            public Sorted(
                Func<TItem, TItem> getComparablePartOfLeftItem,
                Func<TItem, TItem> getComparablePartOfRightItem,
                IComparer<TItem> comparer)
                : base(getComparablePartOfLeftItem, getComparablePartOfRightItem, comparer) { }
        }
    }
}
