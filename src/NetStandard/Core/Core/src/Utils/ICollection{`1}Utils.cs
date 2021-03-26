// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Utils
{
    public static class ICollectionGenericUtils
    {
        public static CollectionType AddItemAndReturnList<CollectionType, T>(CollectionType source, T item)
            where CollectionType : ICollection<T>
        {
            source.Add(item);
            return source;
        }

        public static ListType AddItemRangeAndReturnList<ListType, ItemType>(ListType target, IEnumerable<ItemType> source)
            where ListType : ICollection<ItemType>
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            source = source ?? throw new ArgumentNullException(nameof(source));

            foreach (var item in source) {
                target.Add(item);
            }

            return target;
        }
    }
}
