// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Extensions;
using Teronis.ComponentModel;

namespace Teronis.Collections.Synchronization
{
    public abstract class SynchronizableCollectionBase<TItem, TNewItem> : Collection<TItem>, ISynchronizedCollection<TItem>, INotifyPropertyChanged, INotifyPropertyChanging
    {
        /* Related to observable collection. */
        private const string CountString = "Count";
        /// <summary>
        /// See https://docs.microsoft.com/en-us/archive/blogs/xtof/binding-to-indexers.
        /// </summary>
        private const string IndexerName = "Item[]";

        public event PropertyChangedEventHandler? PropertyChanged {
            add => PropertyChangeComponent.PropertyChanged += value;
            remove => PropertyChangeComponent.PropertyChanged -= value;
        }

        public event PropertyChangingEventHandler? PropertyChanging {
            add => PropertyChangeComponent.PropertyChanging += value;
            remove => PropertyChangeComponent.PropertyChanging -= value;
        }

        public event EventHandler? CollectionSynchronizing;

        public event NotifyNotifyCollectionModifiedEventHandler<TItem>? CollectionModified;

        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        public event EventHandler? CollectionSynchronized;

        protected PropertyChangeComponent PropertyChangeComponent { get; private set; }

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        internal protected virtual ICollectionChangeHandler<TItem> CollectionChangeHandler { get; private set; }

        private bool itemTypeImplementsDisposable;

        public SynchronizableCollectionBase(IList<TItem> list, CollectionChangeHandler<TItem>.IBehaviour handler)
            : base(list) =>
            OnInitialize(decoupledHandler: handler);

        public SynchronizableCollectionBase(IList<TItem> list)
            : base(list) =>
            OnInitialize();

        public SynchronizableCollectionBase(ICollectionChangeHandler<TItem> handler)
            : base(handler.Items) =>
            OnInitialize(dependencyInjectedHandler: handler);

        public SynchronizableCollectionBase() =>
            OnInitialize();

        [MemberNotNull(
            nameof(PropertyChangeComponent),
            nameof(itemTypeImplementsDisposable),
            nameof(CollectionChangeHandler))]
        private void OnInitialize(
            ICollectionChangeHandler<TItem>? dependencyInjectedHandler = null,
            CollectionChangeHandler<TItem>.IBehaviour? decoupledHandler = null)
        {
            PropertyChangeComponent = new PropertyChangeComponent(this);
            itemTypeImplementsDisposable = typeof(IDisposable).IsAssignableFrom(typeof(TItem));
            CollectionChangeHandler = dependencyInjectedHandler ?? new CollectionChangeHandler<TItem>(Items, decoupledHandler);
        }

        internal void InvokeCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        internal void InvokeCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        protected override void InsertItem(int index, TItem item)
        {
            PropertyChangeComponent.OnPropertyChanging(
                CountString,
                IndexerName);

            base.InsertItem(index, item);

            PropertyChangeComponent.OnPropertyChanged(
                CountString,
                IndexerName);
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
            PropertyChangeComponent.OnPropertyChanging(
                CountString,
                IndexerName);

            if (itemTypeImplementsDisposable) {
                DisposeItem(index);
            }

            base.RemoveItem(index);

            PropertyChangeComponent.OnPropertyChanged(
                CountString,
                IndexerName);
        }

        protected override void ClearItems()
        {
            PropertyChangeComponent.OnPropertyChanging(
                CountString,
                IndexerName);

            if (itemTypeImplementsDisposable) {
                for (int index = Count - 1; index >= 0; index--) {
                    DisposeItem(index);
                }
            }

            base.ClearItems();

            PropertyChangeComponent.OnPropertyChanged(
                CountString,
                IndexerName);
        }

        protected virtual CollectionModifiedEventArgs<TItem> CreateCollectionModifiedEventArgs(ICollectionModification<TItem, TItem> modification) =>
            new CollectionModifiedEventArgs<TItem>(modification);

        protected void InvokeCollectionModified(ICollectionModification<TItem, TItem> collectionModification)
        {
            if (collectionChanged is null && CollectionModified is null) {
                return;
            }

            var collectionChangedEventArgs = CreateCollectionModifiedEventArgs(collectionModification);
            CollectionModified?.Invoke(this, collectionChangedEventArgs);
            collectionChanged?.Invoke(this, collectionChangedEventArgs);
        }

        public SynchronizedDictionary<TKey, TItem> CreateSynchronizedDictionary<TKey>(Func<TItem, TKey> getItemKey, IEqualityComparer<TKey> keyEqualityComparer)
            where TKey : notnull =>
            new SynchronizedDictionary<TKey, TItem>(this, getItemKey, keyEqualityComparer);

        public SynchronizedDictionary<KeyType, TItem> CreateSynchronizedDictionary<KeyType>(Func<TItem, KeyType> getItemKey)
            where KeyType : notnull =>
            new SynchronizedDictionary<KeyType, TItem>(this, getItemKey);
    }
}
