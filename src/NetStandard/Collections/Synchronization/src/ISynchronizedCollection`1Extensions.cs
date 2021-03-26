// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public static class ISynchronizedCollection_1Extensions
    {
        public static SynchronizedDictionary<KeyType, ItemType> CreateSynchronizedDictionary<ItemType, KeyType>(
            this ISynchronizedCollection<ItemType> collection,
            Func<ItemType, KeyType> getItemKey,
            IEqualityComparer<KeyType> keyEqualityComparer)
            where KeyType : notnull =>
            new SynchronizedDictionary<KeyType, ItemType>(collection, getItemKey, keyEqualityComparer);

        public static SynchronizedDictionary<KeyType, ItemType> CreateSynchronizedDictionary<ItemType, KeyType>(
            this ISynchronizedCollection<ItemType> collection,
            Func<ItemType, KeyType> getItemKey)
            where KeyType : notnull =>
            new SynchronizedDictionary<KeyType, ItemType>(collection, getItemKey);
    }
}
