using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.Collections.ObjectModel
{
    public class SortableKeyCollection<KeyType, ItemType> : KeyedCollection<KeyType, ItemType>
    {
        private const string DelegateIsNullErrorMessage = "Delegate passed cannot be null.";
        private Func<ItemType, KeyType> _getKeyForItemFunction;

        public SortableKeyCollection(Func<ItemType, KeyType> getKeyForItemFunction) : base()
            => _getKeyForItemFunction = getKeyForItemFunction ?? throw new ArgumentNullException(DelegateIsNullErrorMessage);

        public SortableKeyCollection(Func<ItemType, KeyType> getKeyForItemDelegate, IEqualityComparer<KeyType> comparer) : base(comparer)
            => _getKeyForItemFunction = getKeyForItemDelegate ?? throw new ArgumentNullException(DelegateIsNullErrorMessage);

        protected override KeyType GetKeyForItem(ItemType item) =>
            _getKeyForItemFunction(item);

        public void SortByKeys() => 
            SortByKeys(Comparer<KeyType>.Default);

        public void SortByKeys(IComparer<KeyType> keyComparer) => 
            new DelegatedComparer<ItemType>((x, y) => 
                keyComparer.Compare(GetKeyForItem(x), GetKeyForItem(y)));

        public void SortByKeys(Comparison<KeyType> keyComparison) => 
            Sort(new DelegatedComparer<ItemType>((x, y) => 
                keyComparison(GetKeyForItem(x), GetKeyForItem(y))));

        public void Sort() => 
            Sort(Comparer<ItemType>.Default);

        public void Sort(Comparison<ItemType> comparison) => 
            Sort(new DelegatedComparer<ItemType>((x, y) => 
                comparison(x, y)));

        public void Sort(IComparer<ItemType> comparer) => 
            (Items as List<ItemType>)?.Sort(comparer);
    }
}
