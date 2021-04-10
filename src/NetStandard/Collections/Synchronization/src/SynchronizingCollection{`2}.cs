// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizingCollection<TSuperItem, TSubItem> : SynchronizingCollectionBase<TSuperItem, TSubItem>
        where TSuperItem : notnull
        where TSubItem : notnull
    {
        private Func<TSuperItem, TSubItem> createSubItemHandler;

        public SynchronizingCollection(Func<TSuperItem, TSubItem> createSubItemHandler, Options? options)
            : base(options) =>
            OnInitialize(createSubItemHandler);

        public SynchronizingCollection(Func<TSuperItem, TSubItem> createSubItemHandler, IEqualityComparer<TSuperItem> equalityComparer)
            : base(equalityComparer) =>
            OnInitialize(createSubItemHandler);

        public SynchronizingCollection(Func<TSuperItem, TSubItem> createSubItemHandler, IComparer<TSuperItem> equalityComparer, bool descended)
            : base(equalityComparer, descended) =>
            OnInitialize(createSubItemHandler);

        public SynchronizingCollection(Func<TSuperItem, TSubItem> createSubItemHandler) :
            base() =>
            OnInitialize(createSubItemHandler);

        [MemberNotNull(nameof(createSubItemHandler))]
        private void OnInitialize(Func<TSuperItem, TSubItem> createSubItemHandler) =>
            this.createSubItemHandler = createSubItemHandler ?? throw new ArgumentNullException(nameof(createSubItemHandler));

        protected override TSubItem CreateSubItem(TSuperItem superItem) =>
            createSubItemHandler(superItem);
    }
}
