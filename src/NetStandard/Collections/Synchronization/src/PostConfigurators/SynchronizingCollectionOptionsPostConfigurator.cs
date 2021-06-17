// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization.PostConfigurators
{
    internal class SynchronizingCollectionOptionsPostConfigurator
    {
        public readonly static SynchronizingCollectionOptionsPostConfigurator Default = new SynchronizingCollectionOptionsPostConfigurator();

        public void PostConfigure<TItem>(
            ISynchronizingCollectionItemsOptions<TItem> itemsOptions,
            out ICollectionChangeHandler<TItem> collectionChangeHandler,
            Func<IList<TItem>, ISynchronizedCollection<TItem>> synchronizedItemsFactory,
            out ISynchronizedCollection<TItem> synchronizedItems)
            where TItem : notnull
        {
            SynchronizableCollectionItemsOptionsPostConfigurator.Default.PostConfigure(itemsOptions, out collectionChangeHandler);

            var itemOptionsSynchronizedItems = itemsOptions.SynchronizedItems;

            if (!(itemOptionsSynchronizedItems is null)) {
                synchronizedItems = itemOptionsSynchronizedItems;
                return;
            }

            synchronizedItems = synchronizedItemsFactory(collectionChangeHandler.Items);
            itemsOptions.SetItems(synchronizedItems, collectionChangeHandler);
        }
    }
}

