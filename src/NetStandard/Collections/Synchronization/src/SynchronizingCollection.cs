using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Changes;
using Teronis.Extensions;
using Teronis.ObjectModel.Parenting;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// Remember: All childs are parents (because child extends parent), so all childs are a subset of parent / parent is a superset of child.
    /// </summary>
    /// <typeparam name="SubItemType"></typeparam>
    /// <typeparam name="SuperItemType"></typeparam>
    public abstract class SynchronizingCollection<SubItemType, SuperItemType> : ViewModelBase, ISynchronizeCollection<SuperItemType>, INotifyCollectionModified<SubItemType, SuperItemType>, IHaveParents
        where SubItemType : notnull
        where SuperItemType : notnull
    {
        internal static ICollectionModification<SubItemType, SuperItemType> ReplaceOldSuperItemsByOldSubItems(
            ICollectionModification<SuperItemType, SuperItemType> superItemsSuperItemsModification,
            ICollection<SubItemType> subItems)
        {
            var oldItems = superItemsSuperItemsModification.GetItemsBeginningFromOldIndex(subItems);
            var subItemsSuperItemsModification = superItemsSuperItemsModification.CopyWithOtherItems(oldItems, superItemsSuperItemsModification.NewItems);
            return subItemsSuperItemsModification;
        }

        internal static ICollectionModification<SuperItemType, SuperItemType> CreateOldSuperItemsNewSuperItemsCollectionModification(
            ICollectionModification<SubItemType, SuperItemType> oldSubItemsNewSuperItemsModification,
            ICollection<SuperItemType> superItems)
        {
            var oldItems = oldSubItemsNewSuperItemsModification.GetItemsBeginningFromNewIndex(superItems);
            var contentContentChange = oldSubItemsNewSuperItemsModification.CopyWithOtherItems(oldItems, oldSubItemsNewSuperItemsModification.NewItems);
            return contentContentChange;
        }

        public event EventHandler? CollectionSynchronizing;
        public event EventHandler? CollectionSynchronized;
        public event NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType>? CollectionModified;

        public SubItemCollection SubItems { get; private set; }
        public SuperItemCollection SuperItems { get; private set; }
        public IEqualityComparer<SuperItemType> SuperItemEqualityComparer { get; private set; }

        public SynchronizingCollection(IEqualityComparer<SuperItemType>? superItemEqualityComparer)
        {
            SuperItemEqualityComparer = superItemEqualityComparer ?? EqualityComparer<SuperItemType>.Default;
            /* Initialize collections. */
            SubItems = new SubItemCollection(this);
            SuperItems = new SuperItemCollection(this);
        }

        public SynchronizingCollection() : this(default) { }

        protected abstract SubItemType CreateSubItem(SuperItemType superItem);

        public CollectionModifiedImitator<ToBeImitatedCollectionType> CreateCollectionModifiedImitator<ToBeImitatedCollectionType>(ToBeImitatedCollectionType toBeImitatedCollection)
            where ToBeImitatedCollectionType : INotifyCollectionSynchronizing<SuperItemType>, INotifyCollectionModified<SuperItemType>, INotifyCollectionSynchronized<SuperItemType> =>
            new CollectionModifiedImitator<ToBeImitatedCollectionType>(this, toBeImitatedCollection);

        protected virtual void ApplyCollectionItemRemove(in ApplyingCollectionModificationBundle modificationBundle)
        {
            var contentContentChange = modificationBundle.OldSuperItemsNewSuperItemsModification;
            var oldItems = contentContentChange.OldItems;

            if (oldItems is null) {
                throw new ArgumentException("No old content-content-items were given although a remove collection change action has been triggered.");
            }

            var oldItemsCount = oldItems.Count;
            var oldIndex = contentContentChange.OldIndex;

            for (var oldItemIndex = oldItemsCount - 1; oldItemIndex >= 0; oldItemIndex--) {
                var removeIndex = oldItemIndex + oldIndex;
#if DEBUG
                var removingContent = SuperItems[removeIndex];
                var oldContent = oldItems[oldItemIndex];

                if (!SuperItemEqualityComparer.Equals(removingContent, oldContent)) {
                    throw new Exception("Removing item is not equals old item that should be removed instead");
                }
#endif
                SubItems.RemoveAt(removeIndex);
                SuperItems.RemoveAt(removeIndex);
            }
        }

        protected virtual void ApplyCollectionItemAdd(in ApplyingCollectionModificationBundle modificationBundle)
        {
            var itemContentChange = modificationBundle.OldSubItemsNewSuperItemsModification;
            var newContentContentItems = itemContentChange.NewItems;

            if (newContentContentItems is null) {
                throw new ArgumentException("No new item-content-items were given although an add collection change action has been triggered.");
            }

            var newItemsCount = newContentContentItems.Count;

            for (int itemIndex = 0; itemIndex < newItemsCount; itemIndex++) {
                var content = newContentContentItems[itemIndex];
                var itemInsertIndex = itemContentChange.NewIndex + itemIndex;
                var item = CreateSubItem(content);

                SubItems.Insert(itemInsertIndex, item);
                SuperItems.Insert(itemInsertIndex, content);
            }
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="modificationBundle"></param>
        protected virtual void ApplyCollectionItemMove(in ApplyingCollectionModificationBundle modificationBundle)
        {
            var modification = modificationBundle.OldSubItemsNewSuperItemsModification;

            void moveItem<T>(IList<T> list)
            {
                if (list is ObservableCollection<SubItemType> observableList) {
                    observableList.Move(modification.OldIndex, modification.NewIndex);
                } else {
                    list.Move(modification.OldIndex, modification.NewIndex);
                }
            }

            moveItem(SubItems);
            moveItem(SuperItems);
        }

        /// <summary>
        /// This method has no code inside and is ready for being overriden.
        /// </summary>
        protected virtual void ApplyCollectionItemReplace(in ApplyingCollectionModificationBundle modificationBundle)
        { }

        protected virtual void ApplyCollectionReset(in ApplyingCollectionModificationBundle modificationBundle)
        {
            var modification = modificationBundle.OldSubItemsNewSuperItemsModification;
            var newSuperItems = modification.NewItems ?? throw new ArgumentNullException(nameof(modification.NewItems));
            var newSubItems = newSuperItems.Select(x => CreateSubItem(x));

            static void resetList<T>(IList<T> list, IEnumerable<T> newList)
            {
                list.Clear();

                if (list is List<T> typedList) {
                    typedList.AddRange(newList);
                } else {
                    foreach (var item in newList) {
                        list.Add(item);
                    }
                }
            }

            resetList(SubItems, newSubItems);
            resetList(SuperItems, newSuperItems);
        }

        protected virtual void ApplyCollectionModificationBundle(in ApplyingCollectionModificationBundle modificationBundle)
        {
            switch (modificationBundle.OldSuperItemsNewSuperItemsModification.Action) {
                case NotifyCollectionChangedAction.Remove:
                    ApplyCollectionItemRemove(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Add:
                    ApplyCollectionItemAdd(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Move:
                    ApplyCollectionItemMove(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ApplyCollectionItemReplace(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ApplyCollectionReset(modificationBundle);
                    break;
            }
        }

        protected CollectionModificationBundle<SubItemType, SuperItemType> CreateAppliedCollectionModificationBundle(in ApplyingCollectionModificationBundle applyingModificationBundle)
        {
            /*  We want transform new-super-items to new-sub-items because 
             *  they have been created previously if any is existing. */
            var oldSubItemsNewSuperItemsModification = applyingModificationBundle.OldSubItemsNewSuperItemsModification;
            var newSubItems = oldSubItemsNewSuperItemsModification.GetItemsBeginningFromNewIndex(SubItems);
            var oldSubItemsNewSubItemsModification = oldSubItemsNewSuperItemsModification.CopyWithOtherItems(oldSubItemsNewSuperItemsModification.OldItems, newSubItems);

            var appliedModificationBundle = new CollectionModificationBundle<SubItemType, SuperItemType>(
                oldSubItemsNewSubItemsModification,
                applyingModificationBundle.OldSubItemsNewSuperItemsModification,
                applyingModificationBundle.OldSuperItemsNewSuperItemsModification);

            return appliedModificationBundle;
        }

        protected void OnCollectionModified(CollectionModifiedEventArgs<SubItemType, SuperItemType> args)
            => CollectionModified?.Invoke(this, args);

        public virtual void ApplyCollectionModification(ICollectionModification<SuperItemType, SuperItemType> contentContentChange)
        {
            var oldSubItemsNewSuperItemsModification = ReplaceOldSuperItemsByOldSubItems(contentContentChange, SubItems);
            var applyingModificationBundle = new ApplyingCollectionModificationBundle(oldSubItemsNewSuperItemsModification, contentContentChange);
            ApplyCollectionModificationBundle(applyingModificationBundle);
            var appliedModificationBundle = CreateAppliedCollectionModificationBundle(applyingModificationBundle);
            var args = new CollectionModifiedEventArgs<SubItemType, SuperItemType>(appliedModificationBundle);
            OnCollectionModified(args);
        }

        protected void OnCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        protected void OnCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        /// <summary>
        /// Synchronizes collection with <paramref name="items"/>. Triggers <see cref="CollectionSynchronized"/>.
        /// </summary>
        /// <param name="items"></param>
        public virtual void SynchronizeCollection(IEnumerable<SuperItemType> items)
        {
            OnCollectionSynchronizing();
            items ??= Enumerable.Empty<SuperItemType>();

            //var cachedCollection = new List<TItem>(Collection);
            //var list = items.Take(5).ToList();
            //items = list.ReturnInValue((x) => x.Shuffle()).Take(ThreadSafeRandom.Next(0, list.Count + 1));

            var modifications = SuperItems.GetCollectionModifications(items, SuperItemEqualityComparer)
#if DEBUG
                .ToList()
#endif
            ;

            foreach (var modification in modifications) {
                ApplyCollectionModification(modification);
            }

            OnCollectionSynchronized();
        }

        protected readonly struct ApplyingCollectionModificationBundle
        {
            public ICollectionModification<SubItemType, SuperItemType> OldSubItemsNewSuperItemsModification { get; }
            public ICollectionModification<SuperItemType, SuperItemType> OldSuperItemsNewSuperItemsModification { get; }

            public ApplyingCollectionModificationBundle(ICollectionModification<SubItemType, SuperItemType> oldSubItemsNewSuperItemsModification,
                ICollectionModification<SuperItemType, SuperItemType> oldSuperItemsNewSuperItemsModification)
            {
                OldSubItemsNewSuperItemsModification = oldSubItemsNewSuperItemsModification;
                OldSuperItemsNewSuperItemsModification = oldSuperItemsNewSuperItemsModification;
            }
        }

        public abstract class ItemCollection<ItemType> : Collection<ItemType>, INotifyCollectionSynchronizing<ItemType>, INotifyCollectionModified<ItemType>, INotifyCollectionChanged, INotifyCollectionSynchronized<ItemType>
        {
            public event EventHandler? CollectionSynchronizing;

            public event NotifyNotifyCollectionModifiedEventHandler<ItemType>? CollectionModified;

            event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged {
                add => collectionChanged += value;
                remove => collectionChanged -= value;
            }

            public event EventHandler? CollectionSynchronized;

            private event NotifyCollectionChangedEventHandler? collectionChanged;

            public ItemCollection(INotifyCollectionModified<SubItemType, SuperItemType> collectionModificationNotifier) =>
                collectionModificationNotifier.CollectionModified += CollectionModificationNotifier_CollectionModified;

            protected abstract NotifyCollectionModifiedEventArgs<ItemType> CreateNotifyCollectionModifiedEventArgs(ICollectionModificationBundle<SubItemType, SuperItemType> modificationBundle);

            private void CollectionModificationNotifier_CollectionModified(object sender, CollectionModifiedEventArgs<SubItemType, SuperItemType> args)
            {
                if (collectionChanged is null && CollectionModified is null) {
                    return;
                }

                var collectionChangedEventArgs = CreateNotifyCollectionModifiedEventArgs(args);
                CollectionModified?.Invoke(this, collectionChangedEventArgs);
                collectionChanged?.Invoke(this, collectionChangedEventArgs);
            }
        }

        public class SubItemCollection : ItemCollection<SubItemType>, INotifyCollectionChanged
        {
            public SubItemCollection(INotifyCollectionModified<SubItemType, SuperItemType> collectionModificationNotifier)
                : base(collectionModificationNotifier) { }

            protected override NotifyCollectionModifiedEventArgs<SubItemType> CreateNotifyCollectionModifiedEventArgs(ICollectionModificationBundle<SubItemType, SuperItemType> modificationBundle) =>
                new NotifyCollectionModifiedEventArgs<SubItemType>(modificationBundle.OldSubItemsNewSubItemsModification);
        }

        public class SuperItemCollection : ItemCollection<SuperItemType>, INotifyCollectionChanged
        {
            public SuperItemCollection(INotifyCollectionModified<SubItemType, SuperItemType> collectionModificationNotifier)
                : base(collectionModificationNotifier) { }

            protected override NotifyCollectionModifiedEventArgs<SuperItemType> CreateNotifyCollectionModifiedEventArgs(ICollectionModificationBundle<SubItemType, SuperItemType> modificationBundle) =>
                new NotifyCollectionModifiedEventArgs<SuperItemType>(modificationBundle.OldSuperItemsNewSuperItemsModification);
        }

        public class CollectionModifiedImitator<ToBeImitatedCollectionType>
            where ToBeImitatedCollectionType : INotifyCollectionSynchronizing<SuperItemType>, INotifyCollectionModified<SuperItemType>, INotifyCollectionSynchronized<SuperItemType>
        {
            private readonly SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection;

            public CollectionModifiedImitator(SynchronizingCollection<SubItemType, SuperItemType> synchronizingCollection, ToBeImitatedCollectionType toBeImitatedCollection)
            {
                toBeImitatedCollection.CollectionSynchronizing += ToBeImitatedCollection_CollectionSynchronizing;
                toBeImitatedCollection.CollectionModified += ToBeImitatedCollection_CollectionModified;
                toBeImitatedCollection.CollectionSynchronized += ToBeImitatedCollection_CollectionSynchronized;
                this.synchronizingCollection = synchronizingCollection;
            }

            private void ToBeImitatedCollection_CollectionModified(object sender, NotifyCollectionModifiedEventArgs<SuperItemType> e) =>
                synchronizingCollection.ApplyCollectionModification(e);

            private void ToBeImitatedCollection_CollectionSynchronizing(object sender, EventArgs e) =>
                synchronizingCollection.OnCollectionSynchronizing();

            private void ToBeImitatedCollection_CollectionSynchronized(object sender, EventArgs e) =>
                synchronizingCollection.OnCollectionSynchronized();
        }
    }
}
