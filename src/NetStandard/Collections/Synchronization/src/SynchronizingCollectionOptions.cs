// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Synchronization
{
    public static class SynchronizingCollectionOptions
    {
        public static SynchronizingCollectionOptions<TSuperItem, TSubItem> Create<TSuperItem, TSubItem>()
            where TSuperItem : notnull
            where TSubItem : notnull =>
            new SynchronizingCollectionOptions<TSuperItem, TSubItem>();
    }
}
