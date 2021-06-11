// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    internal interface ISynchronizingCollectionItemsOptions<TItem>
    {
        ICollectionChangeHandler<TItem>? CollectionChangeHandler { get; }
        ISynchronizedCollection<TItem>? Items { get; }

        void SetItems(CollectionChangeHandler<TItem>.IBehaviour modificationHandler);
        void SetItems(IList<TItem> modificationHandlerItems, CollectionChangeHandler<TItem>.IBehaviour modificationHandler);
        void SetItems(ISynchronizedCollection<TItem> items, ICollectionChangeHandler<TItem> modificationHandler);
    }
}
