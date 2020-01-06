using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Teronis.Collections.ObjectModel
{
    public class SortableKeyCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
    {
        private const string DELEGATE_NULL_EXCEPTION_MESSAGE = "Delegate passed cannot be null";
        private Func<TItem, TKey> _getKeyForItemFunction;

        public SortableKeyCollection(Func<TItem, TKey> getKeyForItemFunction) : base()
            => _getKeyForItemFunction = getKeyForItemFunction ?? throw new ArgumentNullException(DELEGATE_NULL_EXCEPTION_MESSAGE);

        public SortableKeyCollection(Func<TItem, TKey> getKeyForItemDelegate, IEqualityComparer<TKey> comparer) : base(comparer)
            => _getKeyForItemFunction = getKeyForItemDelegate ?? throw new ArgumentNullException(DELEGATE_NULL_EXCEPTION_MESSAGE);

        protected override TKey GetKeyForItem(TItem item) => _getKeyForItemFunction(item);

        public void SortByKeys() => SortByKeys(Comparer<TKey>.Default);
        public void SortByKeys(IComparer<TKey> keyComparer) => new DelegatedComparer<TItem>((x, y) => keyComparer.Compare(GetKeyForItem(x), GetKeyForItem(y)));
        public void SortByKeys(Comparison<TKey> keyComparison) => Sort(new DelegatedComparer<TItem>((x, y) => keyComparison(GetKeyForItem(x), GetKeyForItem(y))));
        public void Sort() => Sort(Comparer<TItem>.Default);
        public void Sort(Comparison<TItem> comparison) => Sort(new DelegatedComparer<TItem>((x, y) => comparison(x, y)));
        public void Sort(IComparer<TItem> comparer) => (Items as List<TItem>)?.Sort(comparer);
    }
}
