// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class ICollectionModificationParametersTools
    {
        public static bool MoveRangeContains(ICollectionModificationParameters modification, int index) =>
            CollectionTools.MoveRangeContains(modification.OldIndex, modification.NewIndex, modification.OldItemsCount!.Value, index);
    }
}
