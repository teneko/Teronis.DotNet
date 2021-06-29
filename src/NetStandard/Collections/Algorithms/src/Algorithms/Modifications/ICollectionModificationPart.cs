// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionModificationPart<out TOwnerNewItem, out TOwnerOldItem, out TItem, out TOtherItem>
    {
        ICollectionModification<TOwnerNewItem, TOwnerOldItem> Owner { get; }
        ICollectionModificationPart<TOwnerNewItem, TOwnerOldItem, TOtherItem, TItem> OtherPart { get; }
        IReadOnlyList<TItem>? Items { get; }
        int Index { get; }
    }
}
