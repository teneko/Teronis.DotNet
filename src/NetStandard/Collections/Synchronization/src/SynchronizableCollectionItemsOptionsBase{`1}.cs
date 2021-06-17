// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public abstract class SynchronizableCollectionItemsOptionsBase<TDerived, TItem> : ISynchronizableCollectionItemsOptions<TItem>
        where TDerived : SynchronizableCollectionItemsOptionsBase<TDerived, TItem>
    {
        public ICollectionChangeHandler<TItem>? CollectionChangeHandler { get; protected set; }

        public TDerived SetItems(ICollectionChangeHandler<TItem>? changeHandler)
        {
            CollectionChangeHandler = changeHandler;
            return (TDerived)this;
        }

        public TDerived SetItems(IList<TItem> items)
        {
            CollectionChangeHandler = new CollectionChangeHandler<TItem>(items);
            return (TDerived)this;
        }

        /// <summary>
        /// Sets <see cref="CollectionChangeHandler"/> by creating a 
        /// <see cref="CollectionChangeHandler{ItemType}"/>
        /// with <paramref name="changeHandlerItems"/> and <paramref name="changeHandlerBehaviour"/>.
        /// <see cref="SynchronizableCollectionBase{ItemType, NewItemType}"/>.
        /// </summary>
        /// <param name="changeHandlerItems"></param>
        /// <param name="changeHandlerBehaviour"></param>
        public TDerived SetItems(IList<TItem> changeHandlerItems, CollectionChangeHandler<TItem>.IBehaviour changeHandlerBehaviour)
        {
            if (changeHandlerItems is null) {
                throw new ArgumentNullException(nameof(changeHandlerItems));
            }

            if (changeHandlerBehaviour is null) {
                throw new ArgumentNullException(nameof(changeHandlerBehaviour));
            }

            CollectionChangeHandler = new CollectionChangeHandler<TItem>(changeHandlerItems, changeHandlerBehaviour);
            return (TDerived)this;
        }

        /// <summary>
        /// Sets <see cref="CollectionChangeHandler"/> by creating a 
        /// <see cref="CollectionChangeHandler{ItemType}"/>
        /// with new <see cref="List{T}"/> and <paramref name="changeHandlerBehaviour"/>.
        /// <see cref="SynchronizableCollectionBase{ItemType, NewItemType}"/>.
        /// </summary>
        /// <param name="changeHandlerBehaviour"></param>
        public TDerived SetItems(CollectionChangeHandler<TItem>.IBehaviour changeHandlerBehaviour) =>
            SetItems(new List<TItem>(), changeHandlerBehaviour);

        #region ISynchronizableCollectionItemsOptions<TItem>

        void ISynchronizableCollectionItemsOptions<TItem>.SetItems(ICollectionChangeHandler<TItem>? changeHandler) =>
            SetItems(changeHandler);

        #endregion
    }
}
