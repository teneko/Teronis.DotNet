// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class IListGenericExtensions
    {
        public static void Shuffle<T>(this IList<T> list) =>
            IListGenericUtils.Shuffle(list);

        public static void Move<T>(this IList<T> list, int fromIndex, int toIndex)
        {
            if (fromIndex < 0) {
                throw new ArgumentOutOfRangeException(nameof(fromIndex), "The index you are trying to move from is smaller than zero.");
            }

            if (toIndex < 0) {
                throw new ArgumentOutOfRangeException(nameof(toIndex), "The index you are trying to move to is smaller than zero.");
            }

            var listCount = list.Count;

            if (fromIndex > listCount) {
                throw new ArgumentOutOfRangeException(nameof(fromIndex), "The index you are trying to move from is exceeding the collection range.");
            }

            if (toIndex > listCount) {
                throw new ArgumentOutOfRangeException(nameof(toIndex), "The index you are trying to move to is exceeding the collection range.");
            }

            ListUtils.MoveItem(
                fromIndex,
                toIndex,
                (index) => list[index],
                (index) => list.RemoveAt(index),
                (index, item) => list.Insert(index, item));
        }

        public static void Move<T>(this IList<T> list, T from, T to)
        {
            var fromIndex = list.IndexOf(from);
            var toIndex = list.IndexOf(to);
            Move(list, fromIndex, toIndex);
        }

        public static void Move<T>(this IList<T> list, int fromIndex, int toIndex, int count)
        {
            if (fromIndex < 0) {
                throw new ArgumentOutOfRangeException(nameof(fromIndex), "The index you are trying to move from is smaller than zero.");
            }

            if (toIndex < 0) {
                throw new ArgumentOutOfRangeException(nameof(toIndex), "The index you are trying to move to is smaller than zero.");
            }

            var listCount = list.Count;

            if (fromIndex + count > listCount) {
                throw new ArgumentOutOfRangeException(nameof(count), "The index range you are trying to move is exceeding the collection range.");
            }

            if (toIndex > listCount) {
                throw new ArgumentOutOfRangeException(nameof(toIndex), "The index you are trying to move to is exceeding the collection range.");
            }

            ListUtils.MoveItems(
                fromIndex,
                toIndex,
                count,
                index => list[index],
                list.RemoveAt,
                list.Insert);
        }
    }
}
