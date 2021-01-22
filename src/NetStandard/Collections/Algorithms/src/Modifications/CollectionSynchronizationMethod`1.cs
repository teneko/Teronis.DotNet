using System;
using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public sealed class CollectionSynchronizationMethod<ItemType>
        where ItemType : notnull
    {
        public class Sequential : CollectionSynchronizationMethod<ItemType, ItemType, ItemType>.Sequential, ICollectionSynchronizationMethod<ItemType>
        {
            public Sequential(
                Func<ItemType, ItemType> getComparablePartOfLeftItem,
                Func<ItemType, ItemType> getComparablePartOfRightItem,
                IEqualityComparer<ItemType> equalityComparer)
                : base(getComparablePartOfLeftItem, getComparablePartOfRightItem, equalityComparer) { }
        }

        public class Sorted : CollectionSynchronizationMethod<ItemType, ItemType, ItemType>.Sorted, ICollectionSynchronizationMethod<ItemType>
        {
            public Sorted(
                Func<ItemType, ItemType> getComparablePartOfLeftItem,
                Func<ItemType, ItemType> getComparablePartOfRightItem,
                SortedCollectionOrder collectionOrder,
                IComparer<ItemType> comparer)
                : base(getComparablePartOfLeftItem, getComparablePartOfRightItem, collectionOrder, comparer) { }
        }
    }
}
