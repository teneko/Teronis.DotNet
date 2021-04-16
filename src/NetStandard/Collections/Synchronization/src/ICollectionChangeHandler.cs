// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// <para>
    /// A collection change handler. It handles every action of <see cref="NotifyCollectionChangedAction"/>
    /// for one item except for <see cref="NotifyCollectionChangedAction.Reset"/> and
    /// <see cref="NotifyCollectionChangedAction.Move"/>.
    /// </para>
    /// <para>The naming in this class originates from <see cref="NotifyCollectionChangedAction"/>.</para>
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface ICollectionChangeHandler<TItem>
    {
        IList<TItem> Items { get; }
        /// <summary>
        /// Indicates whether <see cref="ReplaceItem(int, Func{TItem})"/> is functional and ready to be called.
        /// If it is <see langword="false"/> then it is intended that <see cref="ReplaceItem(int, Func{TItem})"/>
        /// is not called.
        /// </summary>
        bool CanReplaceItem { get; }

        void InsertItem(int insertAt, TItem item);
        void MoveItems(int fromIndex, int toIndex, int count);
        void RemoveItem(int removeAt);
        void ReplaceItem(int replaceAt, Func<TItem> getNewItem);
        void Reset();
    }
}
