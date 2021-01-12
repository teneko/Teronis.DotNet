using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Teronis.Collections.Changes;
using Teronis.Collections.Generic;
using Teronis.Collections.Synchronization.Utils;
using Teronis.Extensions;
using Teronis.ObjectModel.Parenting;
using Teronis.ViewModels;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// A synchronizing collection that is itself a view model.
    /// The bindable collection is <see cref="SubItems"/>.
    /// Remember: All childs are parents (because child extends parent), so all childs are a subset of parent / parent is a superset of child.
    /// </summary>
    /// <typeparam name="SubItemType"></typeparam>
    /// <typeparam name="SuperItemType"></typeparam>
    public abstract partial class SynchronizingCollection<SubItemType, SuperItemType> : ViewModelBase, ISynchronizeCollection<SuperItemType>, INotifyCollectionModified<SubItemType, SuperItemType>, IHaveParents
        where SubItemType : notnull
        where SuperItemType : notnull
    {
        internal static CollectionModification<SubItemType, SuperItemType> ReplaceOldSuperItemsByOldSubItems(
            ICollectionModification<SuperItemType, SuperItemType> superItemsSuperItemsModification,
            ICollection<SubItemType> subItems)
        {
            var oldItems = superItemsSuperItemsModification.GetItemsBeginningFromOldIndex(subItems);
            var subItemsSuperItemsModification = superItemsSuperItemsModification.CopyWithOtherItems(oldItems, superItemsSuperItemsModification.NewItems);
            return subItemsSuperItemsModification;
        }

        internal static CollectionModification<SuperItemType, SuperItemType> CreateOldSuperItemsNewSuperItemsCollectionModification(
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

        public SubItemCollection SubItems => subItems;
        public SuperItemCollection SuperItems => superItems;
        public IEqualityComparer<SuperItemType> SuperItemEqualityComparer { get; private set; }
        /// <summary>
        /// The replace strategy is used in <see cref="ApplyCollectionItemReplace(in ApplyingCollectionModificationBundle)"/>.
        /// </summary>
        public CollectionItemReplaceStrategyBase? ItemReplaceStrategy { get; set; }

        private readonly SubItemCollection subItems;
        private readonly SuperItemCollection superItems;

        public SynchronizingCollection(IEqualityComparer<SuperItemType>? superItemEqualityComparer)
        {
            SuperItemEqualityComparer = superItemEqualityComparer ?? EqualityComparer<SuperItemType>.Default;
            /* Initialize collections. */
            subItems = new SubItemCollection(this);
            superItems = new SuperItemCollection(this);
        }

        public SynchronizingCollection() : this(default) { }

        protected abstract SubItemType CreateSubItem(SuperItemType superItem);

        /// <summary>
        /// Creates for this instance a collection synchronisation mirror. The collection modifications from <paramref name="toBeImitatedCollection"/> are 
        /// forwarded to <see cref="ApplyCollectionModification(ICollectionModification{SuperItemType, SuperItemType}, NotifyCollectionChangedAction[])"/>
        /// of this instance.
        /// </summary>
        /// <typeparam name="ToBeImitatedCollectionType">The generic constraint that represents the to be imitated collection.</typeparam>
        /// <param name="toBeImitatedCollection">The foreign collection that is about to be imitated related to its modifications.</param>
        /// <returns>A collection synchronisation mirror.</returns>
        public CollectionSynchronisationMirror<ToBeImitatedCollectionType> CreateCollectionSynchronisationMirror<ToBeImitatedCollectionType>(ToBeImitatedCollectionType toBeImitatedCollection)
            where ToBeImitatedCollectionType : INotifyCollectionSynchronizing<SuperItemType>, INotifyCollectionModified<SuperItemType>, INotifyCollectionSynchronized<SuperItemType> =>
            new CollectionSynchronisationMirror<ToBeImitatedCollectionType>(this, toBeImitatedCollection);

        protected virtual void ApplyCollectionItemRemove(in ApplyingCollectionModificationBundle modificationBundle)
        {
            var contentContentChange = modificationBundle.OldSuperItemsNewSuperItemsModification;
            var oldItems = contentContentChange.OldItems;

            if (oldItems is null) {
                throw new ArgumentException("No old items were given although a remove collection modification action has been triggered.");
            }

            var oldItemsCount = oldItems.Count;
            var oldIndex = contentContentChange.OldIndex;

            for (var oldItemIndex = oldItemsCount - 1; oldItemIndex >= 0; oldItemIndex--) {
                var removeIndex = oldItemIndex + oldIndex;
                Debug.Assert(SuperItemEqualityComparer.Equals(superItems[removeIndex], oldItems[oldItemIndex]), "Removing item is not equals old item that should be removed instead");
                subItems.RemoveAt(removeIndex);
                superItems.RemoveAt(removeIndex);
            }
        }

        protected virtual void ApplyCollectionItemAdd(in ApplyingCollectionModificationBundle modificationBundle)
        {
            var oldSubItemsNewSuperItems = modificationBundle.OldSubItemsNewSuperItemsModification;
            var newSuperItems = oldSubItemsNewSuperItems.NewItems;

            if (newSuperItems is null) {
                throw new ArgumentException("No new items were given although an add collection modification action has been triggered.");
            }

            var newItemsCount = newSuperItems.Count;

            for (int itemIndex = 0; itemIndex < newItemsCount; itemIndex++) {
                var content = newSuperItems[itemIndex];
                var itemInsertIndex = oldSubItemsNewSuperItems.NewIndex + itemIndex;
                var item = CreateSubItem(content);

                subItems.Insert(itemInsertIndex, item);
                superItems.Insert(itemInsertIndex, content);
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

            moveItem(subItems);
            moveItem(superItems);
        }

        /// <summary>
        /// Uses <see cref="SynchronizingCollection{SubItemType, SuperItemType}.ItemReplaceStrategy"/> and calls 
        /// <see cref="CollectionItemReplaceStrategyBase.ApplyCollectionItemReplace{SubItemType, SuperItemType}(SynchronizingCollection{SubItemType, SuperItemType}, ICollectionModification{SubItemType, SuperItemType}, ICollectionModification{SuperItemType, SuperItemType})"/>
        /// .
        /// </summary>
        protected virtual void ApplyCollectionItemReplace(in ApplyingCollectionModificationBundle modificationBundle) =>
            ItemReplaceStrategy?.ApplyCollectionItemReplace(this, modificationBundle.OldSubItemsNewSuperItemsModification, modificationBundle.OldSuperItemsNewSuperItemsModification);

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

            resetList(subItems, newSubItems);
            resetList(superItems, newSuperItems);
        }

        protected virtual void ApplyCollectionModificationBundle(in ApplyingCollectionModificationBundle modificationBundle, params NotifyCollectionChangedAction[] allowedActions)
        {
            var hasAnyAction = allowedActions is null ? true : allowedActions.Length == 0;

            switch (modificationBundle.OldSuperItemsNewSuperItemsModification.Action) {
                case NotifyCollectionChangedAction.Remove when hasAnyAction || (allowedActions?.Contains(NotifyCollectionChangedAction.Remove) ?? true):
                    ApplyCollectionItemRemove(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Add when hasAnyAction || (allowedActions?.Contains(NotifyCollectionChangedAction.Add) ?? true):
                    ApplyCollectionItemAdd(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Move when hasAnyAction || (allowedActions?.Contains(NotifyCollectionChangedAction.Move) ?? true):
                    ApplyCollectionItemMove(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Replace when hasAnyAction || (allowedActions?.Contains(NotifyCollectionChangedAction.Replace) ?? true):
                    ApplyCollectionItemReplace(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Reset when hasAnyAction || (allowedActions?.Contains(NotifyCollectionChangedAction.Reset) ?? true):
                    ApplyCollectionReset(modificationBundle);
                    break;
            }
        }

        protected CollectionModificationBundle<SubItemType, SuperItemType> CreateAppliedCollectionModificationBundle(in ApplyingCollectionModificationBundle applyingModificationBundle)
        {
            /*  We want transform new-super-items to new-sub-items because 
             *  they have been created previously if any is existing. */
            var oldSubItemsNewSuperItemsModification = applyingModificationBundle.OldSubItemsNewSuperItemsModification;
            var newSubItems = oldSubItemsNewSuperItemsModification.GetItemsBeginningFromNewIndex(subItems);
            var oldSubItemsNewSubItemsModification = oldSubItemsNewSuperItemsModification.CopyWithOtherItems(oldSubItemsNewSuperItemsModification.OldItems, newSubItems);

            var appliedModificationBundle = new CollectionModificationBundle<SubItemType, SuperItemType>(
                oldSubItemsNewSubItemsModification,
                applyingModificationBundle.OldSubItemsNewSuperItemsModification,
                applyingModificationBundle.OldSuperItemsNewSuperItemsModification);

            return appliedModificationBundle;
        }

        protected void OnCollectionModified(CollectionModifiedEventArgs<SubItemType, SuperItemType> args)
            => CollectionModified?.Invoke(this, args);

        protected virtual void ApplyCollectionModification(ICollectionModification<SuperItemType, SuperItemType> superItemsModification, params NotifyCollectionChangedAction[] allowedActions)
        {
            var oldSubItemsNewSuperItemsModification = ReplaceOldSuperItemsByOldSubItems(superItemsModification, subItems);
            var applyingModificationBundle = new ApplyingCollectionModificationBundle(oldSubItemsNewSuperItemsModification, superItemsModification);
            ApplyCollectionModificationBundle(applyingModificationBundle, allowedActions);
            var appliedModificationBundle = CreateAppliedCollectionModificationBundle(applyingModificationBundle);
            var args = new CollectionModifiedEventArgs<SubItemType, SuperItemType>(appliedModificationBundle);
            OnCollectionModified(args);
        }

        protected void OnCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        protected void OnCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        /// <summary>
        /// Synchronizes collection with <paramref name="items"/>.
        /// </summary>
        /// <param name="items"></param>
        public virtual void SynchronizeCollection(IEnumerable<SuperItemType> items)
        {
            OnCollectionSynchronizing();
            items ??= Enumerable.Empty<SuperItemType>();

            //var cachedCollection = new List<TItem>(Collection);
            //var list = items.Take(5).ToList();
            //items = list.ReturnInValue((x) => x.Shuffle()).Take(ThreadSafeRandom.Next(0, list.Count + 1));

            var modifications = superItems.GetCollectionModifications(items, SuperItemEqualityComparer)
#if DEBUG
                .ToList()
#endif
            ;

            foreach (var modification in modifications) {
                ApplyCollectionModification(modification);
            }

            OnCollectionSynchronized();
        }

        /// <summary>
        /// Updates existing items with <paramref name="items"/>. 
        /// <br/>Depending on <see cref="ItemReplaceStrategy"/> and 
        /// <see cref="ApplyCollectionItemReplace(in ApplyingCollectionModificationBundle)"/>
        /// this can mean for example to replace super items or update sub items.
        /// </summary>
        /// <param name="items">The items used to find updatable items.</param>
        public virtual void UpdateItems(IEnumerable<SuperItemType> items)
        {
            OnCollectionSynchronizing();
            items ??= Enumerable.Empty<SuperItemType>();
            var modifications = superItems.GetCollectionModifications(items, SuperItemEqualityComparer);

            foreach (var tuple in IEnumerableICollectionModificationUtils.YieldTuplesButOnlyReplaceModificationWithInitialOldIndex(modifications)) {
                if (tuple.Modification.OldItems is null) {
                    throw new InvalidOperationException("The old items were null.");
                }

                var initialOldIndex = tuple.InitialOldIndex;
                var oldItems = SuperItems.Skip(initialOldIndex).Take(tuple.Modification.OldItems.Count).ToList();

                var newModification = tuple.Modification.CopyWithOtherValues(
                    oldItems: oldItems,
                    oldIndex: initialOldIndex,
                    newIndex: initialOldIndex);

                ApplyCollectionModification(newModification, NotifyCollectionChangedAction.Replace);
            }

            OnCollectionSynchronized();
        }

        /// <summary>
        /// Adds not existing items from <paramref name="items"/> and updates existing items 
        /// with <paramref name="items"/>.
        /// <br/>Depending on <see cref="ItemReplaceStrategy"/> and/or 
        /// <see cref="ApplyCollectionItemReplace(in ApplyingCollectionModificationBundle)"/>
        /// this can mean for example to replace super items or update sub items.
        /// </summary>
        /// <br/>The items that are not added or updated will be at the end of the list.
        /// <param name="items">The items used to find addable and updatable items.</param>
        public virtual void AddAndUpdateItems(IEnumerable<SuperItemType> items)
        {
            OnCollectionSynchronizing();
            items ??= Enumerable.Empty<SuperItemType>();
            var modifications = superItems.GetCollectionModifications(items, SuperItemEqualityComparer);

            foreach (var modification in modifications) {
                // This is algorithm dependent. The remove modification are coming at last.
                if (modification.Action == NotifyCollectionChangedAction.Remove) {
                    break;
                }

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
    }
}
