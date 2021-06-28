// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    public class CollectionChangeHandler<TItem> : ICollectionChangeHandler<TItem>
    {
        public event CollectionChangeRedirectInsert<TItem>? RedirectInsert;
        public event CollectionChangeRedirectMove? RedirectMove;
        public event CollectionChangeRedirectRemove? RedirectRemove;
        public event CollectionChangeRedirectReplace<TItem>? RedirectReplace;
        public event CollectionChangeRedirectReset? RedirectReset;

        public virtual IList<TItem> Items { get; } = null!;
        public virtual ICollectionChangeBehaviour Handler { get; } = null!;

        protected CollectionChangeHandler() { }

        public CollectionChangeHandler(IList<TItem> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Handler = DefaultCollectionChangeBehaviour.Default;
        }

        public CollectionChangeHandler(IList<TItem> items, ICollectionChangeBehaviour? behvaiour)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Handler = behvaiour ?? DefaultCollectionChangeBehaviour.Default;
        }

        public virtual bool CanReplaceItem =>
            Handler.CanReplaceItem;

        public virtual void InsertItem(int insertAt, TItem item, bool preventInsertRedirect = false)
        {
            if (preventInsertRedirect) {
                Handler.InsertItem(Items, insertAt, item);
            } else {
                RedirectInsert?.Invoke(insertAt, item);
            }
        }

        public virtual void RemoveItem(int removeAt, bool preventRemoveRedirect = false)
        {
            if (preventRemoveRedirect) {
                Handler.RemoveItem(Items, removeAt);
            } else {
                RedirectRemove?.Invoke(removeAt);
            }
        }

        public virtual void MoveItems(int fromIndex, int toIndex, int count, bool preventMoveRedirect = false)
        {
            if (preventMoveRedirect) {
                Handler.MoveItems(Items, fromIndex, toIndex, count);
            } else {
                RedirectMove?.Invoke(fromIndex, toIndex, count);
            }
        }

        public virtual void ReplaceItem(int replaceAt, Func<TItem> getItem, bool preventReplaceRedirect = false)
        {
            if (preventReplaceRedirect) {
                Handler.ReplaceItem(Items, replaceAt, getItem);
            } else {
                RedirectReplace?.Invoke(replaceAt, getItem);
            }
        }

        public virtual void ResetItems(bool preventResetRedirect = false)
        {
            if (preventResetRedirect) {
                Items.Clear();
            } else {
                RedirectReset?.Invoke();
            }
        }

        public interface ICollectionChangeBehaviour
        {
            /// <summary>
            /// Indicates whether <see cref="ReplaceItem(IList{TItem}, int, Func{TItem})"/> is functional and ready to be called.
            /// If it is <see langword="false"/> then it is intended that <see cref="ReplaceItem(IList{TItem}, int, Func{TItem})"/>
            /// is not called.
            /// </summary>
            bool CanReplaceItem { get; }

            /// <summary>
            /// Inserts the item at the given index.
            /// </summary>
            /// <param name="list"></param>
            /// <param name="insertAt"></param>
            /// <param name="item"></param>
            void InsertItem(IList<TItem> list, int insertAt, TItem item);
            /// <summary>
            /// Removes item at given index.
            /// </summary>
            /// <param name="list"></param>
            /// <param name="removeAt"></param>
            void RemoveItem(IList<TItem> list, int removeAt);
            /// <summary>
            /// Moves the items between <paramref name="fromIndex"/> and <paramref name="fromIndex"/> + <paramref name="count"/> to <paramref name="toIndex"/>.
            /// </summary>
            /// <param name="list"></param>
            /// <param name="fromIndex"></param>
            /// <param name="toIndex"></param>
            /// <param name="count"></param>
            void MoveItems(IList<TItem> list, int fromIndex, int toIndex, int count);
            /// <summary>
            /// Replaces the item at index <paramref name="replaceAt"/> by the item you get from <paramref name="getNewItem"/>.
            /// </summary>
            /// <param name="list"></param>
            /// <param name="replaceAt"></param>
            /// <param name="getNewItem"></param>
            void ReplaceItem(IList<TItem> list, int replaceAt, Func<TItem> getNewItem);
            /// <summary>
            /// Clears the list.
            /// </summary>
            /// <param name="list"></param>
            void Reset(IList<TItem> list);
        }

        /// <summary>
        /// The default behaviour for changing the collection on collection-change-notifications.
        /// Insert, remove, move and reset do as they are named. Only the replace-functionality is
        /// disabled.
        /// </summary>
        public class DefaultCollectionChangeBehaviour : ICollectionChangeBehaviour
        {
            public static DefaultCollectionChangeBehaviour Default = new DefaultCollectionChangeBehaviour();

            public virtual bool CanReplaceItem { get; } = false;

            public virtual void InsertItem(IList<TItem> list, int insertAt, TItem item) =>
                list.Insert(insertAt, item);

            public virtual void RemoveItem(IList<TItem> list, int removeAt) =>
                list.RemoveAt(removeAt);

            public virtual void MoveItems(IList<TItem> list, int fromIndex, int toIndex, int count) =>
                list.Move(fromIndex, toIndex, count);

            /// <summary>
            /// No behaviour is defined. Thus the body is empty.
            /// </summary>
            /// <param name="list"></param>
            /// <param name="replaceAt"></param>
            /// <param name="getNewItem">A function that gives you the new item.</param>
            public virtual void ReplaceItem(IList<TItem> list, int replaceAt, Func<TItem> getNewItem)
            { }

            public virtual void Reset(IList<TItem> list) =>
                list.Clear();
        }

        /// <summary>
        /// Implements the behaviour to replace an item at a given index on request. It is the contrary
        /// implementation of <see cref="DefaultCollectionChangeBehaviour"/> as it would not replace
        /// existing items.
        /// </summary>
        public class ItemReplacableCollectionChangeBehaviour : DefaultCollectionChangeBehaviour
        {
            public new static ItemReplacableCollectionChangeBehaviour Default = new ItemReplacableCollectionChangeBehaviour();

            public override bool CanReplaceItem =>
                true;

            /// <summary>
            /// Replaces the item at index <paramref name="replaceAt"/> by the item you get from <paramref name="getNewItem"/>.
            /// </summary>
            /// <param name="list"></param>
            /// <param name="replaceAt"></param>
            /// <param name="getNewItem"></param>
            public override void ReplaceItem(IList<TItem> list, int replaceAt, Func<TItem> getNewItem) =>
                list[replaceAt] = getNewItem();
        }
    }
}
