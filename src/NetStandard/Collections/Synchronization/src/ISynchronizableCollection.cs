// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal interface ICollectionSynchronizationContext<ItemType>
    {
        void BeginCollectionSynchronization();
        void GoThroughModification(ICollectionModification<ItemType, ItemType> superItemModification);
        void EndCollectionSynchronization();
    }
}
