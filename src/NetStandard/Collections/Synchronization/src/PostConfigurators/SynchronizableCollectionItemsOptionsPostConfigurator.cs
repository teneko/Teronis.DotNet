// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Synchronization.PostConfigurators
{
    public class SynchronizableCollectionItemsOptionsPostConfigurator
    {
        public readonly static SynchronizableCollectionItemsOptionsPostConfigurator Default = new SynchronizableCollectionItemsOptionsPostConfigurator();

        public void PostConfigure<TItem>(
            ISynchronizableCollectionItemsOptions<TItem> itemsOptions, out ICollectionChangeHandler<TItem> collectionChangeHandler)
            where TItem : notnull
        {
            var itemOptionsCollectionChangeHandler = itemsOptions.CollectionChangeHandler;

            if (!(itemOptionsCollectionChangeHandler is null)) {
                collectionChangeHandler = itemOptionsCollectionChangeHandler;
                return;
            }

            var items = new List<TItem>();
            collectionChangeHandler = new CollectionChangeHandler<TItem>(items);
            itemsOptions.SetItems(collectionChangeHandler);
        }
    }
}
