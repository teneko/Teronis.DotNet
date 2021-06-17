// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.ComponentModel;
using System.Reactive.Subjects;

namespace Teronis.Collections.Synchronization
{
    public abstract class SynchronizableCollectionBase<TItem, TNewItem> : Collection<TItem>, ISynchronizedCollection<TItem>
    {
        /* Related to observable collection. */
        internal protected const string CountString = "Count";
        /// <summary>
        /// See https://docs.microsoft.com/en-us/archive/blogs/xtof/binding-to-indexers.
        /// </summary>
        internal protected const string IndexerName = "Item[]";
        private readonly ICollectionChangeHandler<TItem> changeHandler;

        public event PropertyChangedEventHandler? PropertyChanged {
            add => ChangeComponent.PropertyChanged += value;
            remove => ChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler? PropertyChanging {
            add => ChangeComponent.PropertyChanging += value;
            remove => ChangeComponent.PropertyChanging -= value;
        }

        public event EventHandler? CollectionSynchronizing;
        public event NotifyNotifyCollectionModifiedEventHandler<TItem>? CollectionModified;

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        public event EventHandler? CollectionSynchronized;

        protected PropertyChangeComponent ChangeComponent { get; private set; }

        // We take the Subject<> implementation because it provides full thread-safety.
        private Subject<ICollectionModification<TItem, TItem>> collectionModificationSubject;

        public SynchronizableCollectionBase(ICollectionChangeHandler<TItem> changeHandler, IReadOnlyCollectionItemsOptions options)
            : base(changeHandler.Items)
        {
            Initialize();
            this.changeHandler = changeHandler;
            changeHandler.RedirectInsert += ChangeHandler_RedirectInsert;
            changeHandler.RedirectRemove += ChangeHandler_RedirectRemove;
            changeHandler.RedirectReplace += ChangeHandler_RedirectReplace;
            changeHandler.RedirectMove += ChangeHandler_RedirectMove;
            changeHandler.RedirectReset += ChangeHandler_RedirectReset;
        }

        private void ChangeHandler_RedirectInsert(int insertAt, TItem item) =>
            InsertItem(insertAt, item);

        private void ChangeHandler_RedirectRemove(int removeAt) =>
            RemoveItem(removeAt);

        private void ChangeHandler_RedirectReplace(int replaceAt, Func<TItem> getNewItem) =>
            SetItem(replaceAt, getNewItem());

        private void ChangeHandler_RedirectMove(int fromIndex, int toIndex, int count) =>
            MoveItems(fromIndex, toIndex, count);

        private void ChangeHandler_RedirectReset() =>
            ClearItems();

        public SynchronizableCollectionBase()
        {
            changeHandler = new CollectionChangeHandler<TItem>(Items);
            Initialize();
        }

        [MemberNotNull(nameof(ChangeComponent), nameof(collectionModificationSubject))]
        private void Initialize()
        {
            ChangeComponent = new PropertyChangeComponent(this);
            collectionModificationSubject = new Subject<ICollectionModification<TItem, TItem>>();
        }

        protected virtual void OnCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        protected virtual void OnCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        /// <summary>
        /// Notifies all handlers of CollectionChanged and CollectionModified and all subscribed observers.
        /// Early returns if no handlers for CollectionChanged, CollectionModified attached and no observers have been subscribed.
        /// </summary>
        /// <param name="collectionModification"></param>
        protected virtual void OnCollectionModified(ICollectionModification<TItem, TItem> collectionModification)
        {
            if (collectionChanged is null && CollectionModified is null && !collectionModificationSubject.HasObservers) {
                return;
            }

            var collectionChangedEventArgs = CreateCollectionModifiedEventArgs(collectionModification);
            collectionChanged?.Invoke(this, collectionChangedEventArgs);
            CollectionModified?.Invoke(this, collectionChangedEventArgs);

            if (collectionModificationSubject.HasObservers) {
                collectionModificationSubject.OnNext(collectionModification);
            }
        }

        protected virtual void OnBeforeAddItem(int itemIndex, TItem item) =>
            ChangeComponent.OnPropertyChanging(CountString, IndexerName);

        protected virtual void OnAfterAddItem(int itemIndex, TItem item) =>
            ChangeComponent.OnPropertyChanged(CountString, IndexerName);

        protected override void InsertItem(int itemIndex, TItem item)
        {
            OnBeforeAddItem(itemIndex, item);
            changeHandler.InsertItem(itemIndex, item, preventInsertRedirect: true);
            var modification = CollectionModification.ForAdd<TItem, TItem>(itemIndex, item);
            OnCollectionModified(modification);
            OnAfterAddItem(itemIndex, item);
        }

        protected virtual void OnBeforeRemoveItem(int itemIndex) =>
            ChangeComponent.OnPropertyChanging(CountString, IndexerName);

        protected virtual void OnAfterRemoveItem(int itemIndex) =>
            ChangeComponent.OnPropertyChanged(CountString, IndexerName);

        protected override void RemoveItem(int itemIndex)
        {
            OnBeforeRemoveItem(itemIndex);
            var oldItem = Items[itemIndex];
            changeHandler.RemoveItem(itemIndex, preventRemoveRedirect: true);
            var modification = CollectionModification.ForRemove<TItem, TItem>(itemIndex, oldItem);
            OnCollectionModified(modification);
            OnAfterRemoveItem(itemIndex);
        }

        protected virtual void OnBeforeReplaceItem(int itemIndex) =>
            ChangeComponent.OnPropertyChanging(IndexerName);

        protected virtual void OnAfterReplaceItem(int itemIndex) =>
            ChangeComponent.OnPropertyChanged(IndexerName);

        protected override void SetItem(int itemIndex, TItem item)
        {
            OnBeforeReplaceItem(itemIndex);
            var oldItem = Items[itemIndex];
            changeHandler.ReplaceItem(itemIndex, () => item, preventReplaceRedirect: true);
            var modification = CollectionModification.ForReplace(itemIndex, oldItem, item);
            OnCollectionModified(modification);
            OnAfterReplaceItem(itemIndex);
        }

        protected virtual void OnBeforeMoveItems() =>
            ChangeComponent.OnPropertyChanging(IndexerName);

        protected virtual void OnAfterMoveItems() =>
            ChangeComponent.OnPropertyChanged(IndexerName);

        protected virtual void MoveItems(int fromIndex, int toIndex, int count)
        {
            OnBeforeMoveItems();
            changeHandler.MoveItems(fromIndex, toIndex, count, preventMoveRedirect: true);
            var modification = CollectionModification.ForMove<TItem, TItem>(fromIndex, Items, toIndex, count);
            OnCollectionModified(modification);
            OnAfterMoveItems();
        }

        public void Move(int fromIndex, int toIndex, int count) =>
            MoveItems(fromIndex, toIndex, count);

        public void Move(int fromIndex, int toIndex) =>
            MoveItems(fromIndex, toIndex, 1);

        protected virtual void OnBeforeResetItems() =>
            ChangeComponent.OnPropertyChanging(CountString, IndexerName);

        protected virtual void OnAfterResetItems() =>
            ChangeComponent.OnPropertyChanged(CountString, IndexerName);

        protected override void ClearItems()
        {
            OnBeforeResetItems();
            changeHandler.ResetItems(preventResetRedirect: true);
            OnCollectionModified(CollectionModification.ForReset<TItem, TItem>());
            OnAfterResetItems();
        }

        public IDisposable Subscribe(IObserver<ICollectionModification<TItem, TItem>> observer) =>
            collectionModificationSubject.Subscribe(observer);

        protected virtual CollectionModifiedEventArgs<TItem> CreateCollectionModifiedEventArgs(ICollectionModification<TItem, TItem> modification) =>
            new CollectionModifiedEventArgs<TItem>(modification);

        public SynchronizedDictionary<TKey, TItem> CreateSynchronizedDictionary<TKey>(Func<TItem, TKey> getItemKey, IEqualityComparer<TKey> keyEqualityComparer)
            where TKey : notnull =>
            new SynchronizedDictionary<TKey, TItem>(this, getItemKey, keyEqualityComparer);

        public SynchronizedDictionary<KeyType, TItem> CreateSynchronizedDictionary<KeyType>(Func<TItem, KeyType> getItemKey)
            where KeyType : notnull =>
            new SynchronizedDictionary<KeyType, TItem>(this, getItemKey);
    }
}
