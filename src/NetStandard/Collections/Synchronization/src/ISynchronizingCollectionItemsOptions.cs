// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Synchronization
{
    internal interface ISynchronizingCollectionItemsOptions<TItem> : ISynchronizableCollectionItemsOptions<TItem>
    {
        ISynchronizedCollection<TItem>? SynchronizedItems { get; }

        void SetItems(ISynchronizedCollection<TItem> synchronizedItems, ICollectionChangeHandler<TItem> modificationHandler);
    }
}
