// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizingCollection<TSuperItem, TSubItem> : SynchronizingCollectionBase<TSuperItem, TSubItem>
        where TSuperItem : notnull
        where TSubItem : notnull
    {
        private Func<TSuperItem, TSubItem> subItemFactory;

        public SynchronizingCollection(Func<TSuperItem, TSubItem> subItemFactory, SynchronizingCollectionOptions<TSuperItem, TSubItem>? options)
            : base(options) =>
            OnInitialize(subItemFactory);

        public SynchronizingCollection(Func<TSuperItem, TSubItem> subItemFactory) :
            base() =>
            OnInitialize(subItemFactory);

        [MemberNotNull(nameof(subItemFactory))]
        private void OnInitialize(Func<TSuperItem, TSubItem> subItemFactory) =>
            this.subItemFactory = subItemFactory ?? throw new ArgumentNullException(nameof(subItemFactory));

        protected override TSubItem CreateSubItem(TSuperItem superItem) =>
            subItemFactory(superItem);
    }
}
