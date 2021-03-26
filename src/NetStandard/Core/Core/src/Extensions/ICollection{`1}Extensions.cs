// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Extensions
{
    public static class ICollectionGenericExtensions
    {
        public static bool TryAdd<T>(this ICollection<T> list, T item)
        {
            if (!list.Contains(item)) {
                list.Add(item);
                return true;
            }

            return false;
        }

        public static T AddAndReturnItem<T>(this ICollection<T> source, T item)
        {
            source.Add(item);
            return item;
        }
    }
}
