// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Collections.Synchronization
{
    public sealed class SynchronizableCollectionOptions<TItem> : SynchronizingCollectionOptionsBase<SynchronizableCollectionOptions<TItem>, TItem>
        where TItem : notnull
    {
        public OptionsForItems ItemsOptions { get; }

        public SynchronizableCollectionOptions() =>
            ItemsOptions = new OptionsForItems();

        public SynchronizableCollectionOptions<TItem> ConfigureItems(Action<OptionsForItems> configureOptions)
        {
            configureOptions?.Invoke(ItemsOptions);
            return this;
        }

        public class OptionsForItems : SynchronizableCollectionOptionsBase<OptionsForItems, TItem>
        {
            /// <summary>
            /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItems(SynchronizingCollectionBase{SuperItemType, SubItemType}.ApplyingCollectionModifications)"/>
            /// but after the items could have been replaced.
            /// </summary>
            public CollectionUpdateItemDelegate<TItem, TItem>? UpdateItem { get; set; }
        }
    }
}
