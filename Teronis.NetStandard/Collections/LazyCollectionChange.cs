using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections
{
    public class LazyCollectionChange<ItemType, ContentType, TargetType> : ICollectionChange<ItemType, TargetType>
    {
        public NotifyCollectionChangedAction Action
            => change.Action;

        public IReadOnlyList<ItemType> OldItems
            => change.OldItems;

        public int OldIndex
            => change.OldIndex;

        public IReadOnlyList<TargetType> NewItems
            => itemList;

        public int NewIndex
            => change.NewIndex;

        private ICollectionChange<ItemType, ContentType> change;
        private ItemList itemList;

        public LazyCollectionChange(ICollectionChange<ItemType, ContentType> change, Func<ContentType, TargetType> getTarget)
        {
            itemList = new ItemList(change.NewItems, getTarget);
            this.change = change;
        }

        public class ItemList : IReadOnlyList<TargetType> {
            private IList<Lazy<TargetType>> itemList;

            public ItemList(IReadOnlyList<ContentType> contentList, Func<ContentType, TargetType> getTarget)
            {
                itemList = new List<Lazy<TargetType>>(contentList.Count);

                foreach (var newItem in contentList)
                    itemList.Add(new Lazy<TargetType>(() => getTarget(newItem)));
            }

            public TargetType this[int index] 
                => itemList[index].Value;

            public int Count 
                => itemList.Count;

            public bool IsReadOnly
                => itemList.IsReadOnly;

            public IEnumerator<TargetType> GetEnumerator()
                => new Enumerator(itemList);

            IEnumerator IEnumerable.GetEnumerator()
                => new Enumerator(itemList);

            private class Enumerator : IEnumerator<TargetType>
            {
                IEnumerator<Lazy<TargetType>> enumerator;

                public Enumerator(IEnumerable<Lazy<TargetType>> enumerable)
                    => enumerator = enumerable.GetEnumerator();

                public TargetType Current 
                    => enumerator.Current.Value;

                object IEnumerator.Current 
                    => enumerator.Current.Value;

                public void Dispose()
                    => enumerator.Dispose();

                public bool MoveNext()
                    => enumerator.MoveNext();

                public void Reset()
                    => enumerator.Reset();
            }
        }
    }
}
