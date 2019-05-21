using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Extensions.NetStandard;

namespace Teronis.Collections.Generic
{
    public class CollectionSynchronizer<TList, TItem>
        where TList : IList<TItem>
    {
        public event EventHandler<object, CollectionChange<TItem>> CollectionChanged;

        public TList Collection { get; private set; }
        public IEqualityComparer<TItem> EqualityComparer { get; private set; }

        public CollectionSynchronizer(TList collection, IEqualityComparer<TItem> equalityComparer)
        {
            EqualityComparer = equalityComparer ?? EqualityComparer<TItem>.Default;
            Collection = collection;
        }

        public CollectionSynchronizer(TList collection) : this(collection, EqualityComparer<TItem>.Default) { }

        protected virtual void onValueRemoved(CollectionChange<TItem> change)
            => Collection.Remove(change.OldValue);

        protected virtual void onValueAdded(CollectionChange<TItem> change)
            => Collection.Insert(change.NewIndex, change.NewValue);

        protected virtual void onValueMove(CollectionChange<TItem> change)
        {
            if (Collection is ObservableCollection<TItem> observableCollection)
                observableCollection.Move(change.OldIndex, change.NewIndex);
            else
                Collection.Move(change.OldIndex, change.NewIndex);
        }

        /// <summary>
        /// This method has no code inside and is ready for overriding.
        /// </summary>
        protected virtual void onValueReplace(CollectionChange<TItem> change) { }

        public void Synchronize(IEnumerable<TItem> sensors)
        {
            sensors = sensors ?? Enumerable.Empty<TItem>();
            var changes = Collection.GetCollectionChanges(sensors, EqualityComparer);

            foreach (var change in changes) {
                switch (change.ChangeAction) {
                    case NotifyCollectionChangedAction.Remove:
                        onValueRemoved(change);
                        break;
                    case NotifyCollectionChangedAction.Add:
                        onValueAdded(change);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        onValueMove(change);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        onValueReplace(change);
                        break;
                }

                CollectionChanged?.Invoke(this, change);
            }
        }
    }
}
