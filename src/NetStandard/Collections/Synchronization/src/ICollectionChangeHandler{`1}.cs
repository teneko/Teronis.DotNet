// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public delegate void CollectionChangeRedirectInsert<TItem>(int insertAt, TItem item);
    public delegate void CollectionChangeRedirectMove(int fromIndex, int toIndex, int count);
    public delegate void CollectionChangeRedirectRemove(int removeAt);
    public delegate void CollectionChangeRedirectReplace<TItem>(int replaceAt, Func<TItem> getNewItem);
    public delegate void CollectionChangeRedirectReset();

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
        event CollectionChangeRedirectInsert<TItem> RedirectInsert;
        event CollectionChangeRedirectMove RedirectMove;
        event CollectionChangeRedirectRemove RedirectRemove;
        event CollectionChangeRedirectReplace<TItem> RedirectReplace;
        event CollectionChangeRedirectReset RedirectReset;

        IList<TItem> Items { get; }
        /// <summary>
        /// Indicates whether <see cref="ReplaceItem(int, Func{TItem}, bool)"/> is functional and ready to be called.
        /// If it is <see langword="false"/> then it is intended that <see cref="ReplaceItem(int, Func{TItem}, bool)"/>
        /// is not called.
        /// </summary>
        bool CanReplaceItem { get; }

        void InsertItem(int insertAt, TItem item, bool preventInsertRedirect = false);
        void MoveItems(int fromIndex, int toIndex, int count, bool preventMoveRedirect = false);
        void RemoveItem(int removeAt, bool preventRemoveRedirect = false);
        void ReplaceItem(int replaceAt, Func<TItem> getNewItem, bool preventReplaceRedirect = false);
        void ResetItems(bool preventResetRedirect = false);
    }
}
