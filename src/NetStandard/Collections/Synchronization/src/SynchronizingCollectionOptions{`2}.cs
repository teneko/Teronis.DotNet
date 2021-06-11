// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public sealed class SynchronizingCollectionOptions<TSuperItem, TSubItem> : SynchronizingCollectionOptionsBase<SynchronizingCollectionOptions<TSuperItem, TSubItem>, TSuperItem>
        where TSuperItem : notnull
        where TSubItem : notnull
    {
        public OptionsForSuperItems SuperItemsOptions { get; }
        public OptionsForSubItems SubItemsOptions { get; }

        public SynchronizingCollectionOptions()
        {
            SuperItemsOptions = new OptionsForSuperItems();
            SubItemsOptions = new OptionsForSubItems();
        }

        public SynchronizingCollectionOptions<TSuperItem, TSubItem> ConfigureSubItems(Action<OptionsForSuperItems> configureOptions)
        {
            configureOptions?.Invoke(SuperItemsOptions);
            return this;
        }

        public SynchronizingCollectionOptions<TSuperItem, TSubItem> ConfigureSubItems(Action<OptionsForSubItems> configureOptions)
        {
            configureOptions?.Invoke(SubItemsOptions);
            return this;
        }

        public abstract class OptionsForItems<TDerived, TItem> : SynchronizableCollectionOptionsBase<TDerived, TItem>, ISynchronizingCollectionItemsOptions<TItem>
            where TDerived : OptionsForItems<TDerived, TItem>
        {
            public ISynchronizedCollection<TItem>? Items { get; protected set; }

            /// <summary>
            /// Sets <see cref="Items"/> and <see cref="SynchronizableCollectionOptionsBase{TDerived, TItem}.CollectionChangeHandler"/>.
            /// </summary>
            /// <param name="items"></param>
            /// <param name="modificationHandler"></param>
            public TDerived SetItems(ISynchronizedCollection<TItem> items, ICollectionChangeHandler<TItem> modificationHandler)
            {
                Items = items ?? throw new ArgumentNullException(nameof(items));
                CollectionChangeHandler = modificationHandler ?? throw new ArgumentNullException(nameof(modificationHandler));
                return (TDerived)this;
            }

            #region ISynchronizableCollectionOptionsBase<TItem>

            void ISynchronizingCollectionItemsOptions<TItem>.SetItems(ISynchronizedCollection<TItem> items, ICollectionChangeHandler<TItem> modificationHandler) =>
                SetItems(items, modificationHandler);

            void ISynchronizingCollectionItemsOptions<TItem>.SetItems(IList<TItem> modificationHandlerItems, CollectionChangeHandler<TItem>.IBehaviour modificationHandler) =>
                SetItems(modificationHandlerItems, modificationHandler);

            void ISynchronizingCollectionItemsOptions<TItem>.SetItems(CollectionChangeHandler<TItem>.IBehaviour modificationHandler) =>
                SetItems(modificationHandler);

            #endregion
        }

        public sealed class OptionsForSuperItems : OptionsForItems<OptionsForSuperItems, TSuperItem>
        {
            /// <summary>
            /// If not null it is called in <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItems(SynchronizingCollectionBase{SuperItemType, SubItemType}.ApplyingCollectionModifications)"/>
            /// but after the items could have been replaced and before <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.OnAfterReplaceItem(int)"/>.
            /// </summary>
            public CollectionUpdateItemDelegate<TSuperItem, TSubItem>? UpdateItem { get; set; }
        }

        public sealed class OptionsForSubItems : OptionsForItems<OptionsForSubItems, TSubItem>
        {
            /// <summary>
            /// If not null it is called by <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.ReplaceItems(SynchronizingCollectionBase{SuperItemType, SubItemType}.ApplyingCollectionModifications)"/>
            /// but after the items could have been replaced and before <see cref="SynchronizingCollectionBase{SuperItemType, SubItemType}.OnAfterReplaceItem(int)"/>.
            /// <br/>
            /// <br/>(!) Take into regard, that <see cref="OptionsForSuperItems.UpdateItem"/> is called at first if not null.
            /// </summary>
            public CollectionUpdateItemDelegate<TSubItem, TSuperItem>? UpdateItem { get; set; }
        }
    }
}
