// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizableCollectionItemsOptions<TItem> : IReadOnlyCollectionItemsOptions
    {
        ICollectionChangeHandler<TItem>? CollectionChangeHandler { get; }

        void SetItems(ICollectionChangeHandler<TItem>? changeHandler);
    }
}
