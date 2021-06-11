// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    internal class SynchronizableCollectionOptionsPostConfigurator
    {
        public readonly static SynchronizableCollectionOptionsPostConfigurator Default = new SynchronizableCollectionOptionsPostConfigurator();

        public void PostConfigure<TItem>(
            ISynchronizingCollectionItemsOptions<TItem> itemsOptions,
            Func<IList<TItem>, ISynchronizedCollection<TItem>> itemCollectionFactory)
            where TItem : notnull
        {
            if (itemsOptions.Items is null) {
                IList<TItem> itemList;
                ICollectionChangeHandler<TItem> itemModificationHandler;

                if (itemsOptions.CollectionChangeHandler is null) {
                    itemList = new List<TItem>();
                    itemModificationHandler = new CollectionChangeHandler<TItem>(itemList);
                } else {
                    itemList = itemsOptions.CollectionChangeHandler.Items;
                    itemModificationHandler = itemsOptions.CollectionChangeHandler;
                }

                var items = itemCollectionFactory(itemList);
                itemsOptions.SetItems(items, itemModificationHandler);
            }
        }
    }
}
