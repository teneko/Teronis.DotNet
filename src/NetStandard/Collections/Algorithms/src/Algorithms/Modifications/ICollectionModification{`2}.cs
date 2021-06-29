// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionModification<out TNewItem, out TOldItem> : ICollectionModificationInfo
    {
        new int OldIndex { get; }
        ICollectionModificationPart<TNewItem, TOldItem, TOldItem, TNewItem> OldPart { get; }
        IReadOnlyList<TOldItem>? OldItems { get; }

        new int NewIndex { get; }
        ICollectionModificationPart<TNewItem, TOldItem, TNewItem, TOldItem> NewPart { get; }
        IReadOnlyList<TNewItem>? NewItems { get; }
    }
}
