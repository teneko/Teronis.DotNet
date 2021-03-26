// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizingCollection<SuperItemType, SubItemType> : SynchronizingCollectionBase<SuperItemType, SubItemType>
        where SuperItemType : notnull
        where SubItemType : notnull
    {
        private Func<SuperItemType, SubItemType> createSubItemHandler = null!;

        public SynchronizingCollection(Func<SuperItemType, SubItemType> createSubItemHandler, Options? options)
            : base(options) =>
            onConstruction(createSubItemHandler);

        public SynchronizingCollection(Func<SuperItemType, SubItemType> createSubItemHandler, IEqualityComparer<SuperItemType> equalityComparer)
            : base(equalityComparer) =>
            onConstruction(createSubItemHandler);

        public SynchronizingCollection(Func<SuperItemType, SubItemType> createSubItemHandler, IComparer<SuperItemType> equalityComparer, bool descended)
            : base(equalityComparer, descended) =>
            onConstruction(createSubItemHandler);

        public SynchronizingCollection(Func<SuperItemType, SubItemType> createSubItemHandler) :
            base() =>
            onConstruction(createSubItemHandler);

        private void onConstruction(Func<SuperItemType, SubItemType> createSubItemHandler) =>
            this.createSubItemHandler = createSubItemHandler ?? throw new ArgumentNullException(nameof(createSubItemHandler));

        protected override SubItemType CreateSubItem(SuperItemType superItem) =>
            createSubItemHandler(superItem);
    }
}
