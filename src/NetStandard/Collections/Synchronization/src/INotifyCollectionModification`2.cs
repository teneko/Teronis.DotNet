// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Collections.Synchronization;

namespace Teronis.Collections.Algorithms
{
    public interface INotifyCollectionModification<SuperItemType, SubItemType>
    {
        event NotifyCollectionModifiedEventHandler<SuperItemType, SubItemType> CollectionModified;
    }
}
