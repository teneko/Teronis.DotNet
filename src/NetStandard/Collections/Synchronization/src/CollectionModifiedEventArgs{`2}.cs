// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public class CollectionModifiedEventArgs<TSuperItem, TSubItem> : EventArgs
    {
        public ICollectionModification<TSubItem, TSubItem> SubItemModification { get; }
        public ICollectionModification<TSuperItem, TSubItem> SuperSubItemModification { get; }
        public ICollectionModification<TSuperItem, TSuperItem> SuperItemModification { get; }

        public CollectionModifiedEventArgs(
            ICollectionModification<TSubItem, TSubItem> subItemModification,
            ICollectionModification<TSuperItem, TSubItem> superSubItemModification,
            ICollectionModification<TSuperItem, TSuperItem> superItemModification)
        {
            SubItemModification = subItemModification;
            SuperSubItemModification = superSubItemModification;
            SuperItemModification = superItemModification;
        }
    }
}
