// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Extensions;
using Teronis.ObjectModel;

namespace Teronis.Collections.Synchronization
{
    public abstract class SynchronizableCollectionBase<TItem, TNewItem> : Collection<TItem>, ISynchronizedCollection<TItem>, INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler? PropertyChanged {
            add => PropertyChangeComponent.PropertyChanged += value;
            remove => PropertyChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler? PropertyChanging {
            add => PropertyChangeComponent.PropertyChanging += value;
            remove => PropertyChangeComponent.PropertyChanging -= value;
        }

        #region Related to observable collection.

        private const string CountString = "Count";
        /// <summary>
        /// See https://docs.microsoft.com/en-us/archive/blogs/xtof/binding-to-indexers.
        /// </summary>
        private const string IndexerName = "Item[]";

        #endregion

        public event EventHandler? CollectionSynchronizing;

        public event NotifyNotifyCollectionModifiedEventHandler<TItem>? CollectionModified;

        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        public event EventHandler? CollectionSynchronized;

        protected PropertyChangeComponent PropertyChangeComponent { get; private set; } = null!;

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        internal protected virtual CollectionChangeHandler<TItem>.IDependencyInjectedHandler CollectionChangeHandler { get; private set; } = null!;

        private bool itemTypeImplementsDisposable;

        public SynchronizableCollectionBase(IList<TItem> list, CollectionChangeHandler<TItem>.IDecoupledHandler handler)
            : base(list) =>
            onConstruction(decoupledHandler: handler);

        public SynchronizableCollectionBase(IList<TItem> list)
            : base(list) =>
            onConstruction();

        public SynchronizableCollectionBase(CollectionChangeHandler<TItem>.IDependencyInjectedHandler handler)
            : base(handler.Items) =>
            onConstruction(dependencyInjectedHandler: handler);

        public SynchronizableCollectionBase() =>
            onConstruction();

        private void onConstruction(
            CollectionChangeHandler<TItem>.IDependencyInjectedHandler? dependencyInjectedHandler = null,
            CollectionChangeHandler<TItem>.IDecoupledHandler? decoupledHandler = null)
        {
            PropertyChangeComponent = new PropertyChangeComponent(this);
            itemTypeImplementsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(TItem));
            CollectionChangeHandler = dependencyInjectedHandler ?? new CollectionChangeHandler<TItem>.DependencyInjectedHandler(Items, decoupledHandler);
        }

        protected override void InsertItem(int index, TItem item)
        {
            PropertyChangeComponent.OnPropertyChanging(CountString);
            PropertyChangeComponent.OnPropertyChanging(IndexerName);

            base.InsertItem(index, item);

            PropertyChangeComponent.OnPropertyChanged(CountString);
            PropertyChangeComponent.OnPropertyChanged(IndexerName);
        }

        protected override void SetItem(int index, TItem item)
        {
            PropertyChangeComponent.OnPropertyChanging(IndexerName);

            base.SetItem(index, item);

            PropertyChangeComponent.OnPropertyChanged(IndexerName);
        }

        protected virtual void MoveItems(int fromIndex, int toIndex, int count)
        {
            PropertyChangeComponent.OnPropertyChanging(IndexerName);

            Items.Move(fromIndex, toIndex, count);

            PropertyChangeComponent.OnPropertyChanged(IndexerName);
        }

        public void Move(int fromIndex, int toIndex, int count) =>
            MoveItems(fromIndex, toIndex, count);

        public void Move(int fromIndex, int toIndex) =>
            MoveItems(fromIndex, toIndex, 1);

        protected virtual void DisposeItem(int index)
        {
            var item = Items[index];

            if (item is IDisposable disposableItem) {
                disposableItem.Dispose();
            }
        }

        protected override void RemoveItem(int index)
        {
            PropertyChangeComponent.OnPropertyChanging(CountString);
            PropertyChangeComponent.OnPropertyChanging(IndexerName);

            if (itemTypeImplementsDisposable) {
                DisposeItem(index);
            }

            base.RemoveItem(index);

            PropertyChangeComponent.OnPropertyChanged(CountString);
            PropertyChangeComponent.OnPropertyChanged(IndexerName);
        }

        protected override void ClearItems()
        {
            PropertyChangeComponent.OnPropertyChanging(CountString);
            PropertyChangeComponent.OnPropertyChanging(IndexerName);

            if (itemTypeImplementsDisposable) {
                for (int index = Count - 1; index >= 0; index--) {
                    DisposeItem(index);
                }
            }

            base.ClearItems();

            PropertyChangeComponent.OnPropertyChanged(CountString);
            PropertyChangeComponent.OnPropertyChanged(IndexerName);
        }

        protected virtual CollectionModifiedEventArgs<TItem> CreateCollectionModifiedEventArgs(ICollectionModification<TItem, TItem> modification) =>
            new CollectionModifiedEventArgs<TItem>(modification);

        protected void InvokeCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        protected void InvokeCollectionModified(ICollectionModification<TItem, TItem> collectionModifiaction)
        {
            if (collectionChanged is null && CollectionModified is null) {
                return;
            }

            var collectionChangedEventArgs = CreateCollectionModifiedEventArgs(collectionModifiaction);
            CollectionModified?.Invoke(this, collectionChangedEventArgs);
            collectionChanged?.Invoke(this, collectionChangedEventArgs);
        }

        protected void InvokeCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        public SynchronizedDictionary<TKey, TItem> CreateSynchronizedDictionary<TKey>(Func<TItem, TKey> getItemKey, IEqualityComparer<TKey> keyEqualityComparer)
            where TKey : notnull =>
            new SynchronizedDictionary<TKey, TItem>(this, getItemKey, keyEqualityComparer);

        public SynchronizedDictionary<KeyType, TItem> CreateSynchronizedDictionary<KeyType>(Func<TItem, KeyType> getItemKey)
            where KeyType : notnull =>
            new SynchronizedDictionary<KeyType, TItem>(this, getItemKey);
    }
}
