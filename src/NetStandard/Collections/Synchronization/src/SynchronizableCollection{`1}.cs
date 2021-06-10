// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizableCollection<TItem> : SynchronizableCollectionBase<TItem, TItem>, ICollectionSynchronizationContext<TItem>
        where TItem : notnull
    {
        private static Options prepareOptions(ref Options? options)
        {
            options ??= new Options();
            options.CollectionChangeHandler ??= new CollectionChangeHandler<TItem>(new List<TItem>());
            return options;
        }

        public ICollectionSynchronizationMethod<TItem, TItem> SynchronizationMethod { get; private set; } = null!;

        private CollectionUpdateItemDelegate<TItem, TItem>? updateItem;

        public SynchronizableCollection(Options? options)
            : base(prepareOptions(ref options).CollectionChangeHandler!)
        {
            options!.SynchronizationMethod ??= CollectionSynchronizationMethod.Sequential<TItem>();
            SynchronizationMethod = options.SynchronizationMethod;
            updateItem = options.UpdateItem;
        }

        public SynchronizableCollection(
            IList<TItem> items,
            ICollectionSynchronizationMethod<TItem, TItem>? synchronizationMethod) 
            : this(new Options() { SynchronizationMethod = synchronizationMethod }
                .SetItems(items))
        { }

        public SynchronizableCollection(IList<TItem> items)
            : this(new Options().SetItems(items)) { }

        public SynchronizableCollection(ICollectionSynchronizationMethod<TItem, TItem> synchronizationMethod)
            : this(new Options() { SynchronizationMethod = synchronizationMethod }) { }

        public SynchronizableCollection()
            : this(options: null) { }

        public SynchronizableCollection(IList<TItem> items, IEqualityComparer<TItem> equalityComparer) 
            : this(new Options()
                  .SetItems(items)
                  .SetSequentialSynchronizationMethod(equalityComparer))
        { }

        public SynchronizableCollection(IEqualityComparer<TItem> equalityComparer) 
            : this(new Options()
                  .SetSequentialSynchronizationMethod(equalityComparer))
        { }

        public SynchronizableCollection(IList<TItem> items, IComparer<TItem> comparer, bool descended) 
            : this(new Options()
                  .SetItems(items)
                  .SetSortedSynchronizationMethod(comparer, descended))
        { }

        public SynchronizableCollection(IComparer<TItem> comparer, bool descended)
            : this(new Options()
                  .SetSortedSynchronizationMethod(comparer, descended))
        { }

        protected virtual void AddItems(ICollectionModification<TItem, TItem> modification)
        {
            CollectionModificationIterationTools.BeginInsert(modification)
                /// The modification is now null checked.
                .Add((modificationItemIndex, collectionStartIndex) => {
                    var superItem = modification.NewItems![modificationItemIndex];
                    var globalIndex = collectionStartIndex + modificationItemIndex;
                    CollectionChangeHandler.InsertItem(globalIndex, superItem);
                })
                .Iterate();
        }

        protected virtual void RemoveItems(ICollectionModification<TItem, TItem> modification)
        {
            CollectionModificationIterationTools.BeginRemove(modification)
                .Add((modificationItemIndex, collectionStartIndex) => {
                    var globalIndex = collectionStartIndex + modificationItemIndex;
                    CollectionChangeHandler.RemoveItem(globalIndex);
                })
                .Iterate();
        }

        protected virtual void OnBeforeReplaceItem(int replacedItemIndex) { }

        protected virtual void OnAfterReplaceItem(int replacedItemIndex) { }

        protected virtual void ReplaceItems(ICollectionModification<TItem, TItem> modification)
        {
            CollectionModificationIterationTools.BeginReplace(modification)
                .Add((modificationItemIndex, collectionStartIndex) => {
                    var globalIndex = collectionStartIndex + modificationItemIndex;
                    var item = modification.NewItems![modificationItemIndex];

                    TItem getItem() =>
                        item;

                    if (CollectionChangeHandler.CanReplaceItem) {
                        CollectionChangeHandler.ReplaceItem(globalIndex, getItem);
                    }

                    updateItem?.Invoke(CollectionChangeHandler.Items[globalIndex], getItem);
                })
                .Iterate();
        }

        protected virtual void MoveItems(ICollectionModification<TItem, TItem> modification)
        {
            CollectionModificationIterationTools.CheckMove(modification);
            CollectionChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, modification.OldItems!.Count);
        }

        protected virtual void ResetItems(ICollectionModification<TItem, TItem> modification) =>
            CollectionChangeHandler.Reset();

        protected virtual void ProcessModification(ICollectionModification<TItem, TItem> modification)
        {
            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add:
                    AddItems(modification);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(modification);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItems(modification);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItems(modification);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetItems(modification);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftItems"></param>
        /// <param name="rightItems"></param>
        /// <param name="yieldCapabilities"></param>
        /// <param name="batchModifications">Indicates that first all modifications are calculated before they get processed.</param>
        internal void SynchronizeCollection(IEnumerable<TItem> leftItems, IEnumerable<TItem>? rightItems, CollectionModificationsYieldCapabilities yieldCapabilities, bool batchModifications)
        {
            leftItems = leftItems ?? throw new ArgumentNullException(nameof(leftItems));
            var modifications = SynchronizationMethod.YieldCollectionModifications(leftItems, rightItems, yieldCapabilities);

            if (batchModifications) {
                modifications = modifications.ToList();
            }

            var modificationEnumerator = modifications.GetEnumerator();

            if (!modificationEnumerator.MoveNext()) {
                return;
            }

            InvokeCollectionSynchronizing();

            do {
                var modification = modificationEnumerator.Current;
                ProcessModification(modification);
                InvokeCollectionModified(modification);
            } while (modificationEnumerator.MoveNext());

            InvokeCollectionSynchronized();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="yieldCapabilities"></param>
        /// <param name="batchModifications">Indicates that first all modifications are calculated before they get processed.</param>
        internal void SynchronizeCollection(IEnumerable<TItem>? enumerable, CollectionModificationsYieldCapabilities yieldCapabilities, bool batchModifications) =>
            SynchronizeCollection(
                batchModifications
                    ? Items // When we batch modifications, then we do not need to mark it.
                    : Items.AsIList().ToProducedListModificationsNotBatchedMarker(),
                enumerable,
                yieldCapabilities,
                batchModifications);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="batchModifications">Indicates that first all modifications are calculated before they get processed.</param>
        internal void SynchronizeCollection(IEnumerable<TItem>? enumerable, bool batchModifications) =>
            SynchronizeCollection(enumerable, CollectionModificationsYieldCapabilities.All, batchModifications);

        public void SynchronizeCollection(IEnumerable<TItem>? enumerable, CollectionModificationsYieldCapabilities yieldCapabilities) =>
            SynchronizeCollection(enumerable, yieldCapabilities, batchModifications: false);

        public void SynchronizeCollection(IEnumerable<TItem>? enumerable) =>
            SynchronizeCollection(enumerable, yieldCapabilities: CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// Creates for this instance a collection synchronisation mirror. The collection modifications from <paramref name="toBeMirroredCollection"/> are 
        /// forwarded to <see cref="ProcessModification(ICollectionModification{TItem, TItem})"/>
        /// of this instance.
        /// </summary>
        /// <param name="toBeMirroredCollection">The foreign collection that is about to be mirrored related to its modifications.</param>
        /// <returns>A collection synchronization mirror.</returns>
        public SynchronizationMirror<TItem> CreateSynchronizationMirror(ISynchronizedCollection<TItem> toBeMirroredCollection) =>
            new SynchronizationMirror<TItem>(this, toBeMirroredCollection);

        #region ICollectionSynchronizationContext<SuperItemType>

        void ICollectionSynchronizationContext<TItem>.BeginCollectionSynchronization() =>
            InvokeCollectionSynchronizing();

        void ICollectionSynchronizationContext<TItem>.GoThroughModification(ICollectionModification<TItem, TItem> superItemModification) =>
            ProcessModification(superItemModification);

        void ICollectionSynchronizationContext<TItem>.EndCollectionSynchronization() =>
            InvokeCollectionSynchronized();

        #endregion
    }
}
