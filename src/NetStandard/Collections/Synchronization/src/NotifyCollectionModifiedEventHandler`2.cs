// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Synchronization
{
    public delegate void NotifyCollectionModifiedEventHandler<SuperItemType, SubItemType>(object sender, CollectionModifiedEventArgs<SuperItemType, SubItemType> args);
}
